using Microsoft.AspNetCore.Mvc;
using ok.Ai.Tools;

namespace ok.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly LogisticsTool _logisticsTool;

        public AiController(LogisticsTool logisticsTool)
        {
            _logisticsTool = logisticsTool;
        }

        [HttpGet("run-logistics/{scladId}")]
        public IActionResult RunLogistics(int scladId)
        {
            var result = _logisticsTool.Execute(scladId);
            return Ok(result);
        }
    }
}
