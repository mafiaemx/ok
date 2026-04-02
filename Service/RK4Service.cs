using ok.Models;

namespace ok.Service
{
    public class Rk4Service
    {
        private double F(double t, double y, double rate)
        {
            return -rate * (y / 100.0);
        }
        public double RungeKutta4(List<Product> products, double coefConsumption, double timeAhead)
        {
            var sortedProducts=products.OrderBy(p => p.ExpirationDate ?? DateOnly.MaxValue).ToList();
            double h = 0.1;
            double t = 0;
            double totalAmount = (double)sortedProducts.Sum(p => p.Amount);
            while (t < timeAhead)
            {
                double k1 = F(t, totalAmount, coefConsumption);
                double k2 = F(t + h / 2, totalAmount + h * k1 / 2, coefConsumption);
                double k3 = F(t + h / 2, totalAmount + h * k2 / 2, coefConsumption);
                double k4 = F(t + h, totalAmount + h * k3, coefConsumption);
                totalAmount += (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                t += h;
                if (totalAmount <= 0)
                {
                    return totalAmount=0;
                }
            }
            double finalvalue= totalAmount;
            for (int i = sortedProducts.Count - 1; i >= 0; i--)
            {
                double originAmount=(double)sortedProducts[i].Amount;
                if (finalvalue > 0)
                {
                   int amountRemains=Math.Min((int)originAmount, (int)finalvalue);
                    sortedProducts[i].Amount -= (decimal)amountRemains;
                    finalvalue -= amountRemains;
                }
                if (sortedProducts[i].ExpirationDate.HasValue && sortedProducts[i].ExpirationDate.Value.ToDateTime(new TimeOnly(0, 0)) <= DateTime.Now.Add(TimeSpan.FromDays(timeAhead)))
                {
                    finalvalue -= (double)sortedProducts[i].Amount;
                }
                else
                {
                    sortedProducts[i].Amount = 0;
                }
            }
            return totalAmount;
        }
    }
}