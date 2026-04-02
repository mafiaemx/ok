using System;
using Google.OrTools.Graph;

namespace ok.Service
{
    public class ModernFlowService
    {
        public void SolveMinCostFlow()
        {
            MinCostFlow minCostFlow = new MinCostFlow();


            minCostFlow.AddArcWithCapacityAndUnitCost(0, 1, 15, 4);
            minCostFlow.AddArcWithCapacityAndUnitCost(0, 2, 10, 2);
            minCostFlow.AddArcWithCapacityAndUnitCost(1, 2, 5, 1);
            minCostFlow.AddArcWithCapacityAndUnitCost(1, 3, 10, 3);
            minCostFlow.AddArcWithCapacityAndUnitCost(2, 3, 15, 2);

            minCostFlow.SetNodeSupply(0, 20); 
            minCostFlow.SetNodeSupply(3, -20); 

            MinCostFlow.Status status = minCostFlow.Solve();

            if (status == MinCostFlow.Status.OPTIMAL)
            {
                Console.WriteLine($"Мінімальна вартість доставки: {minCostFlow.OptimalCost()}");

                for (int i = 0; i < minCostFlow.NumArcs(); ++i)
                {
                    if (minCostFlow.Flow(i) > 0)
                    {
                        Console.WriteLine($"Маршрут {minCostFlow.Tail(i)} -> {minCostFlow.Head(i)}: відправлено {minCostFlow.Flow(i)} одиниць.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Неможливо розрахувати оптимальний потік для таких умов.");
            }
        }
    }
}