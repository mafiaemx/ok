using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ok.Models;

namespace ok.Service
{
    public class LogisticsService
    {
        private readonly Rk4Service _rk4;
        private readonly BwmService _bwm;
        private readonly AppDbContext _context;

        public LogisticsService(Rk4Service rk4, BwmService bwm, AppDbContext context)
        {
            _rk4 = rk4;
            _bwm = bwm;
            _context = context;
        }

        public DistributionResult DistributeFromWarehouse(int scladId)
        {
            var sclad = _context.Sclads.FirstOrDefault(s => s.Id == scladId);
            var deliveryPoints = _context.DeliveryPoints
                .Include(dp => dp.Demands)
                .ThenInclude(d => d.Product)
                .ToList();

            var warehouseStock = _context.Zaluskies
                .Where(z => z.ScladId == scladId)
                .ToList();

            long totalWarehouseSupply = (long)warehouseStock.Sum(z => z.Quantity);

            var pointInfos = new List<PointResult>();

            foreach (var point in deliveryPoints)
            {
                long required = (long)point.Demands.Sum(d => d.RequiredQuantity);

                double priorityLevel = point.Demands.Any()
                    ? point.Demands.Average(d => (double)d.CalculatedWeight)
                    : 1.0;

                if (required <= 0)
                    continue;

                double distance = 5 + (point.PointId % 20);
                double priorityScore = CalculatePriority(required, priorityLevel, distance);

                pointInfos.Add(new PointResult
                {
                    PointId = point.PointId,
                    Name = point.Name,
                    Required = required,
                    Priority = priorityScore
                });
            }

            pointInfos = pointInfos
                .OrderByDescending(p => p.Priority)
                .ToList();

            long remaining = totalWarehouseSupply;

            foreach (var p in pointInfos)
            {
                if (remaining <= 0)
                    break;

                long allocated = Math.Min(remaining, p.Required);

                p.Allocated = allocated;
                remaining -= allocated;
            }

            return new DistributionResult
            {
                WarehouseName = sclad?.Name ?? "Невідомий склад",
                Points = pointInfos,
                RemainingStock = remaining
            };
        }

       

        private double CalculatePriority(double deficit, double weight, double distance)
        {
            double[] bestToOthers = { 1, 3, 5 };
            double[] othersToWorst = { 5, 3, 1 };

            int bestIndex = 0;
            int worstIndex = 2;

            var weights = _bwm.CalculateWeights(
                bestToOthers,
                othersToWorst,
                bestIndex,
                worstIndex
            );

            double[] criteria =
            {
                deficit,
                weight * 100,
                distance
            };

            return _bwm.CalculateScore(criteria, weights);
        }

        private class PointNeedInfo
        {
            public int PointId { get; set; }
            public long Deficit { get; set; }
            public double PriorityScore { get; set; }
        }
    }
    public class DistributionResult
    {
        public string WarehouseName { get; set; } = "";
        public List<PointResult> Points { get; set; } = new();
        public long RemainingStock { get; set; }
    }

    public class PointResult
    {
        public int PointId { get; set; }
        public string Name { get; set; } = "";
        public long Required { get; set; }
        public long Allocated { get; set; }
        public double Priority { get; set; }
    }
}