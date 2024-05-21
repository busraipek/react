using System.Collections.Generic;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OfficeOpenXml;
using System.IO;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System;
using System.Linq;

namespace Project2.Services
{
    public class FlightDataService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly ILogger<FlightDataService> _logger;
        private readonly string _filePath;

        public FlightDataService(ILogger<FlightDataService> logger)
        {
            _logger = logger;
            _filePath = Path.Combine(Directory.GetCurrentDirectory(),  "FlightData.xlsx");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(1)); // Intervali 10 saniye olarak ayarladık
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            var flightDataList = GetFlightData();
            ExportToExcel(flightDataList);
            _logger.LogInformation("Flight data fetched and exported to Excel at: {time}", DateTimeOffset.Now);
        }

        private List<FlightData> GetFlightData()
        {
            var flightDataList = new List<FlightData>();
            var web = new HtmlWeb();
            var doc = web.Load("https://www.avionio.com/en/airport/saw/departures?ts=1716062400000");

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

        private void ExportToExcel(List<FlightData> flightDataList)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "FlightData.csv");

            // Check if the file exists
            FileInfo file = new FileInfo(filePath);
            ExcelPackage package;
            if (file.Exists)
            {
                // If the file exists, load it
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    package = new ExcelPackage(stream);
                }
            }
            else
            {
                // If the file doesn't exist, create a new package
                package = new ExcelPackage();
            }

            // Get the worksheet or create a new one if it doesn't exist
            var worksheet = package.Workbook.Worksheets.FirstOrDefault(ws => ws.Name == "Flight Data");
            if (worksheet == null)
            {
                worksheet = package.Workbook.Worksheets.Add("Flight Data");

                // Add headers if the worksheet is newly created
                worksheet.Cells["A1"].Value = "Time";
                worksheet.Cells["B1"].Value = "Date";
                worksheet.Cells["C1"].Value = "IATA";
                worksheet.Cells["D1"].Value = "Destination";
                worksheet.Cells["E1"].Value = "Flight";
                worksheet.Cells["F1"].Value = "Airline";
                worksheet.Cells["G1"].Value = "Status";
            }

            // Find the last used row
            int startRow = worksheet.Dimension.End.Row + 1;

            // Add data
            for (int i = 0; i < flightDataList.Count; i++)
            {
                var flight = flightDataList[i];
                worksheet.Cells[startRow + i, 1].Value = flight.Time;
                worksheet.Cells[startRow + i, 2].Value = flight.Date;
                worksheet.Cells[startRow + i, 3].Value = flight.IATA;
                worksheet.Cells[startRow + i, 4].Value = flight.Destination;
                worksheet.Cells[startRow + i, 5].Value = flight.Flight;
                worksheet.Cells[startRow + i, 6].Value = flight.Airline;
                worksheet.Cells[startRow + i, 7].Value = flight.Status;
            }

            // Save the Excel package to the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                package.SaveAs(stream);
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
