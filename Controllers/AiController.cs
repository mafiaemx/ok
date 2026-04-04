using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly AppDbContext _context;

        public AiController(AiOrchestrator orchestrator, LogisticsTool logisticsTool, AppDbContext context)
        {
            _orchestrator = orchestrator;
            _logisticsTool = logisticsTool;
            _context = context;
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

        [HttpGet("warehouse-summary/{scladId}")]
        public async Task<IActionResult> WarehouseSummary(int scladId)
        {
           
            var sclad = await _context.Sclads.FindAsync(scladId);
            if (sclad == null)
                return NotFound("Склад не знайдено");

            var stock = await _context.Zaluskies
                .Where(z => z.ScladId == scladId)
                .Include(z => z.Product)
                .Select(z => new
                {
                    product = z.Product!.Name,
                    quantity = z.Quantity,
                    unit = z.Product!.Unit
                })
                .ToListAsync();

            var distribution = _logisticsTool.GetService().DistributeFromWarehouse(scladId);

            return Ok(new
            {
                warehouse = sclad.Name,
                address = sclad.Address,
                stock,
                distribution = distribution.Points.Select(p => new
                {
                    point = p.Name,
                    pointId = p.PointId,
                    required = p.Required,
                    allocated = p.Allocated,
                    priority = Math.Round(p.Priority, 2)
                }),
                remaining = distribution.RemainingStock
            });
        }
    }
}