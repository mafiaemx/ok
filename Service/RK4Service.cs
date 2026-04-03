using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ok.Models;

namespace ok.Service
{
    public class Rk4Service
    {
        private readonly AppDbContext _context;
        public Rk4Service(AppDbContext context)
        {
            _context = context;
        }
        private double F(double t, double y, double rate)
        {
            return -rate * (y / 100.0);
        }
        public double RungeKutta4(int scladId, double coefConsumption, double timeAhead)
        {
            var zalusky = _context.Zaluskies.
                Include(z => z.Product).
                Where(z => z.ScladId == scladId).
                ToList();
            var sortedProducts = zalusky.
                OrderBy(z => z.Product.ExpirationDate ?? DateOnly.MaxValue).ToList();
            double h = 0.1;
            double t = 0;
            double totalQuantity = sortedProducts.Sum(z => (double)z.Quantity);
            while (t < timeAhead)
            {
                double k1 = F(t, totalQuantity, coefConsumption);
                double k2 = F(t + h / 2, totalQuantity + h * k1 / 2, coefConsumption);
                double k3 = F(t + h / 2, totalQuantity + h * k2 / 2, coefConsumption);
                double k4 = F(t + h, totalQuantity + h * k3, coefConsumption);
                totalQuantity += (h / 6) * (k1 + 2 * k2 + 2 * k3 + k4);
                t += h;
                if (totalQuantity <= 0)
                {
                    return 0;
                }
            }
            double finalvalue= totalQuantity;
            foreach(var pr in sortedProducts)
            {
                double originCount = (double)pr.Quantity;
                if (finalvalue > 0)
                {
                    int amountRemains = Math.Min((int)originCount, (int)finalvalue);
                    pr.Quantity -= amountRemains;
                    finalvalue -= amountRemains;
                }
                if (pr.Product!.ExpirationDate.HasValue)
                {
                    var expireDate = pr.Product.ExpirationDate.Value
                        .ToDateTime(TimeOnly.MinValue);
                    if(expireDate <= DateTime.Now.Add(TimeSpan.FromDays(timeAhead)))
                    {
                        finalvalue -= (double)pr.Quantity;
                        pr.Quantity = 0;
                    }
                }
            }
            return totalQuantity;
        }
    }
}