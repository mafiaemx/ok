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

        public double CalculatePriority(double currentStock, double consumptionRate, double required)
        {
            // прогноз (RK4)
            double forecast = _rk4.RungeKutta4(currentStock, consumptionRate, 5);

            // критерії
            double deficit = Math.Max(0, required - forecast);
            double penalty = 100;
            double distance = 20;

            // BWM
            double[] bestToOthers = { 1, 5, 9 };
            double[] othersToWorst = { 9, 4, 1 };

            var weights = _bwm.CalculateWeights(bestToOthers, othersToWorst);

            double[] criteria = { deficit, penalty, distance };

            return _bwm.CalculateScore(criteria, weights);
        }
    }
}