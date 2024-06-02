using Microsoft.AspNetCore.Mvc;
using Project2;
using Project2.Models;
using Project2.Services;

namespace Project2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PredictionController : ControllerBase
    {
        private readonly MLModelService _mlModelService;

        public PredictionController()
        {
            _mlModelService = new MLModelService();
        }

        [HttpPost]
        [Route("predict")]
        public IActionResult Predict([FromBody] PredictionInput input)
        {
            var output = _mlModelService.Predict(input);
            return Ok(output);
        }
    }
}
