using Microsoft.AspNetCore.Mvc;
using Project2.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project2;

namespace Project2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private readonly IMLModelService _mlModelService;

        public PredictionController(IMLModelService mlModelService)
        {
            _mlModelService = mlModelService;
        }

        [HttpPost("predict")]
        public IActionResult Predict([FromBody] FlightPrediction input)
        {
            var result = _mlModelService.Predict(input);
            return Ok(result);
        }
    }
}