using Microsoft.AspNetCore.Mvc;
using ok.Models;
using ok.Service;
using Microsoft.EntityFrameworkCore;

namespace ok.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogisticsController : ControllerBase
    {
        private readonly LogisticsService _logisticsService;
        private readonly AppDbContext _context;

        public LogisticsController(LogisticsService logisticsService, AppDbContext context)
        {
            _logisticsService = logisticsService;
            _context = context;
        }

        [HttpPost("run-distribution/{scladId}")]
        public async Task<IActionResult> RunFullDistribution(int scladId)
        {
            var sclad = await _context.Sclads.FindAsync(scladId);
            if (sclad == null) return NotFound("Склад не знайдено");

            try
            {
                var plan = _logisticsService.DistributeFromWarehouse(scladId);

                if (plan.Count == 0) return Ok("Розподіл не потрібен, дефіциту немає.");

                foreach (var item in plan)
                {
                    var shipment = new Shipment
                    {
                        ScladId = scladId,
                        PointId = item.Key, 
                        CreatedAt = DateTime.Now
                    };

                    _context.Shipments.Add(shipment);
                    await _context.SaveChangesAsync(); 

                    var shipmentItem = new ShipmentItem
                    {
                        ShipmentId = shipment.Id,
                        ProductId = 1, 
                        Quantity = (decimal)item.Value
                    };
                    _context.ShipmentItems.Add(shipmentItem);
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Логістичну схему оптимізовано та збережено в БД",
                    DistributionPlan = plan
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Критична помилка алгоритму: {ex.Message}");
            }
        }

        [HttpGet("stock-by-point")]
        public async Task<IActionResult> GetStockByPoint()
        {
            var stock = await _context.Zaluskies
                .Include(z => z.Product)
                .Include(z => z.Sclad)
                .ToListAsync();

            return Ok(stock);
        }

        
    }
}