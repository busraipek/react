using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;

namespace Project2.Services
{
    public class FlightWebScraping : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger<FlightWebScraping> _logger;
        private readonly string _connectionString;
        private readonly string[] _urls = new string[]
        {
            "https://www.avionio.com/en/airport/saw/departures",

        };

        public FlightWebScraping(ILogger<FlightWebScraping> logger)
        {
            _logger = logger;
            _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=FlightData;Integrated Security=True;";
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1)); // Intervali 1 saat olarak ayarladık
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            try
            {
                foreach (var url in _urls)
                {
                    var flightDataList = GetFlightData(url);
                    ExportToSqlServer(flightDataList);
                    _logger.LogInformation("Flight data fetched and exported to SQL Server at: {time}", DateTimeOffset.Now);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching or exporting flight data.");
            }
        }

        private List<FlightData> GetFlightData(string url)
        {
            var flightDataList = new List<FlightData>();
            var web = new HtmlWeb();
            var doc = web.Load(url);

            var rows = doc.DocumentNode.SelectNodes("//tr[@class='tt-row ']");
            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var flight = new FlightData
                    {
                        Time = row.SelectSingleNode(".//td[contains(@class, 'tt-t')]")?.InnerText.Trim(),
                        Date = row.SelectSingleNode(".//td[contains(@class, 'tt-d')]")?.InnerText.Trim(),
                        IATA = row.SelectSingleNode(".//td[contains(@class, 'tt-i')]//a")?.InnerText.Trim(),
                        Destination = row.SelectSingleNode(".//td[contains(@class, 'tt-ap')]")?.InnerText.Trim(),
                        Flight = row.SelectSingleNode(".//td[contains(@class, 'tt-f')]//a")?.InnerText.Trim(),
                        Airline = row.SelectSingleNode(".//td[contains(@class, 'tt-al')]")?.InnerText.Trim(),
                        Status = row.SelectSingleNode(".//td[contains(@class, 'tt-s')]")?.InnerText.Trim()
                    };
                    flightDataList.Add(flight);
                }
            }
            return flightDataList;
        }

        private void ExportToSqlServer(List<FlightData> flightDataList)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    foreach (var flight in flightDataList)
                    {
                        var command = new SqlCommand("INSERT INTO departuresdata (Time, Date, IATA, Destination, Flight, Airline, Status) VALUES (@Time, @Date, @IATA, @Destination, @Flight, @Airline, @Status)", conn);

                        command.Parameters.AddWithValue("@Time", flight.Time);
                        command.Parameters.AddWithValue("@Date", flight.Date);
                        command.Parameters.AddWithValue("@IATA", flight.IATA);
                        command.Parameters.AddWithValue("@Destination", flight.Destination);
                        command.Parameters.AddWithValue("@Flight", flight.Flight);
                        command.Parameters.AddWithValue("@Airline", flight.Airline);
                        command.Parameters.AddWithValue("@Status", flight.Status);

                        command.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, "An error occurred while inserting data into SQL Server.");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
