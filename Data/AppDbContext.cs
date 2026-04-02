using Microsoft.EntityFrameworkCore;

namespace YourProjectNamespace.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        
        public DbSet<Sclad> Sclads { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<DeliveryPoint> DeliveryPoints { get; set; }
        public DbSet<Zalusky> Zalusky { get; set; }
        public DbSet<Demand> Demand { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentItem> ShipmentItems { get; set; }
    }
}