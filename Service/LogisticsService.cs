using System;
using System.Collections.Generic;
using System.Linq;
using Google.OrTools.Graph;
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

        public Dictionary<int, long> DistributeFromWarehouse(int scladId)
        {
            var deliveryPoints = _context.DeliveryPoints
                .Include(dp => dp.Demands)
                    .ThenInclude(d => d.Product)
                .ToList();

            var warehouseStock = _context.Zaluskies
                .Where(z => z.ScladId == scladId)
                .ToDictionary(z => z.ProductId, z => (long)z.Quantity);

            MinCostFlow minCostFlow = new MinCostFlow();
            int warehouseNode = 0; 
            long totalDemand = 0;

            var targetsMapping = new Dictionary<int, int>(); 

            for (int i = 0; i < deliveryPoints.Count; i++)
            {
                var point = deliveryPoints[i];
                int storeNode = i + 1; 
                targetsMapping.Add(storeNode, point.PointId);

                double required = (double)point.Demands.Sum(d => d.RequiredQuantity);
                double consumptionRate = 5.0; 
                double forecast = _rk4.RungeKutta4(scladId, consumptionRate, 5);

                long deficit = (long)Math.Max(0, required - forecast);

                double distance = 10.0; 
                double priority = CalculatePriority(deficit, required, distance);

                if (deficit > 0)
                {
                    minCostFlow.SetNodeSupply(storeNode, -deficit);
                    totalDemand += deficit;

                    long edgeCost = (long)Math.Max(1, 1000 - priority);
                    minCostFlow.AddArcWithCapacityAndUnitCost(warehouseNode, storeNode, deficit, edgeCost);
                }
                else
                {
                    minCostFlow.SetNodeSupply(storeNode, 0);
                }
            }

            long totalWarehouseSupply = warehouseStock.Values.Sum();
            long actualSupply = Math.Min(totalWarehouseSupply, totalDemand);
            minCostFlow.SetNodeSupply(warehouseNode, actualSupply);

            MinCostFlow.Status status = minCostFlow.Solve();
            Dictionary<int, long> distributionPlan = new Dictionary<int, long>();

            if (status == MinCostFlow.Status.OPTIMAL)
            {
                for (int i = 0; i < minCostFlow.NumArcs(); ++i)
                {
                    if (minCostFlow.Flow(i) > 0)
                    {
                        int storeNode = minCostFlow.Head(i);
                        int pointId = targetsMapping[storeNode];
                        distributionPlan[pointId] = minCostFlow.Flow(i);
                    }
                }
            }

            return distributionPlan;
        }

        private double CalculatePriority(double deficit, double required, double distance)
        {
            double penalty = 100;

            double[] bestToOthers = { 1, 5, 9 };
            double[] othersToWorst = { 9, 4, 1 };
            int bestIndex = 0;
            int worstIndex = 2;

            var weights = _bwm.CalculateWeights(bestToOthers, othersToWorst, bestIndex, worstIndex);

            double[] criteria = { deficit, penalty, distance };
            return _bwm.CalculateScore(criteria, weights);
        }
    }
}