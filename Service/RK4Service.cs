namespace ok.Service
{
    public class Rk4Service
    {
        private double F(double t, double y, double rate)
        {
            return -rate * (y / 100.0);
        }
        public double RungeKutta4(double currentStock, double consumptionRate, double timeAhead)
        {
            double h = 0.1;
            double t = 0;
            double y = currentStock;
            while (t < timeAhead)
            {
                double k1 = F(t, y, consumptionRate);
                double k2 = F(t + h / 2, y + h * k1 / 2, consumptionRate);
                double k3 = F(t + h / 2, y + h * k2 / 2, consumptionRate);
                double k4 = F(t + h, y + h * k3, consumptionRate);
                y += (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                t += h;
                if (y <= 0)
                {
                    return 0;
                }
            }
            return y;
        }
    }
}