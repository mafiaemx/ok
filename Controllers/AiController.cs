using Microsoft.AspNetCore.Mvc;
using ok.Ai;
using ok.Ai.Tools;
using ok.Models;

namespace ok.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly AiOrchestrator _orchestrator;
        private readonly LogisticsTool _logisticsTool;

        public AiController(AiOrchestrator orchestrator, LogisticsTool logisticsTool)
        {
            _orchestrator = orchestrator;
            _logisticsTool = logisticsTool;
        }

        [HttpPost("run")]
        public async Task<IActionResult> Run([FromBody] AiRequest request)
        {
            if (string.IsNullOrEmpty(request.Prompt))
                return BadRequest("Prompt cannot be empty");

            var result = await _orchestrator.RunAsync(request.Prompt);
            return Ok(result);
        }

        [HttpGet("run-test/{scladId}")]
        public async Task<IActionResult> RunTest(int scladId)
        {
            var result = await _logisticsTool.ExecuteAsync(scladId);
            return Ok(result);
        }
    }
}