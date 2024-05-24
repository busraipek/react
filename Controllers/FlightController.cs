using Microsoft.AspNetCore.Mvc;
using Project2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project2.Controllers
{        
    [ApiController]
    [Route("api/[controller]")]
    public class FlightController : ControllerBase
    {

            private readonly FlightTraining _flightDataService;

            public FlightController(FlightTraining flightDataService)
            {
                _flightDataService = flightDataService;
            }

            [HttpGet]
            public async Task<IActionResult> GetFlights()
            {
                try
                {
                    List<FlightData> flights = await _flightDataService.GetFlightData();
                    return Ok(flights);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message); // Handle errors appropriately
                }
            }

            // Implement a POST endpoint for predictions if using a machine learning model
        }
}
