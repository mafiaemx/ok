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

                if (plan.Points.Count == 0)
                    return Ok("Розподіл не потрібен, попиту немає.");

                foreach (var item in plan.Points)
                {
                    if (item.Allocated <= 0)
                        continue;

                    var shipment = new Shipment
                    {
                        ScladId = scladId,
                        PointId = item.PointId,
                        CreatedAt = DateTime.Now
                    };

                    _context.Shipments.Add(shipment);
                    await _context.SaveChangesAsync();

                    var shipmentItem = new ShipmentItem
                    {
                        ShipmentId = shipment.Id,
                        ProductId = 1,
                        Quantity = (decimal)item.Allocated
                    };

                    _context.ShipmentItems.Add(shipmentItem);
                }

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Логістичну схему оптимізовано",
                    Points = plan.Points,
                    RemainingStock = plan.RemainingStock
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Критична помилка: {ex.Message}");
            }
        }


    }
}