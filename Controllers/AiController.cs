using Microsoft.AspNetCore.Mvc;
using ok.Ai.Tools;
using ok.Service;

namespace ok.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AiController : ControllerBase
    {
        private readonly LogisticsTool _logisticsTool;
        private readonly LlmService _llmService;
       
            
        

        public AiController(LogisticsTool logisticsTool, LlmService llmService)
        {
            _logisticsTool = logisticsTool;
            _llmService = llmService;
        }
        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] string prompt)
        {
            var result = await _llmService.GenerateAsync(prompt);
            return Ok(result);
        }
        [HttpGet("run-logistics/{scladId}")]
        public IActionResult RunLogistics(int scladId)
        {
            var result = _logisticsTool.Execute(scladId);
            return Ok(result);
        }
    }
}
