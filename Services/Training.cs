using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;



namespace Project2.Services
{
        public class FlightTraining
        {
            private readonly string _connectionString;

            public FlightTraining(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("DefaultConnection");
            }

            public async Task<List<FlightData>> GetFlightData()
            {
                List<FlightData> flights = new List<FlightData>();
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    string sql = "SELECT Time, Airline, Flight, Status FROM departuresdata";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                flights.Add(new FlightData
                                {
                                    Time = reader.GetString(0),
                                    Airline = reader.GetString(1),
                                    Flight = reader.GetString(2),
                                    Status = reader.IsDBNull(3) ? null : reader.GetString(3)
                                });
                            }
                        }
                    }
                }
                return flights;
            }
        }
}
