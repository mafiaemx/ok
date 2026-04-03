using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ok.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DeliveryPoint> DeliveryPoints { get; set; }

    public virtual DbSet<Demand> Demands { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Sclad> Sclads { get; set; }

    public virtual DbSet<Shipment> Shipments { get; set; }

    public virtual DbSet<ShipmentItem> ShipmentItems { get; set; }

    public virtual DbSet<Zalusky> Zaluskies { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeliveryPoint>(entity =>
        {
            entity.HasKey(e => e.PointId).HasName("delivery_points_pkey");

            entity.ToTable("delivery_points");

            entity.Property(e => e.PointId).HasColumnName("point_id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Demand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("demand_pkey");

            entity.ToTable("demand");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CalculatedWeight)
                .HasPrecision(18, 4)
                .HasDefaultValueSql("1.0000")
                .HasColumnName("calculated_weight");
            entity.Property(e => e.ForecastedStock)
                .HasPrecision(18, 2)
                .HasColumnName("forecasted_stock");
            entity.Property(e => e.PointId).HasColumnName("point_id");
            entity.Property(e => e.PriorityLevel)
                .HasMaxLength(20)
                .HasDefaultValueSql("'Normal'::character varying")
                .HasColumnName("priority_level");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.RequiredQuantity).HasColumnName("required_quantity");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_at");

            entity.HasOne(d => d.Point).WithMany(p => p.Demands)
                .HasForeignKey(d => d.PointId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("demand_point_id_fkey");

            entity.HasOne(d => d.Product).WithMany(p => p.Demands)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("demand_product_id_fkey");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("products_pkey");

            entity.ToTable("products");

            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Unit).HasColumnName("unit");
        });
        modelBuilder.Entity<Sclad>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sclad_pkey");

            entity.ToTable("sclad");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address).HasColumnName("address");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<Shipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shipments_pkey");

            entity.ToTable("shipments");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.PointId).HasColumnName("point_id");
            entity.Property(e => e.ScladId).HasColumnName("sclad_id");

            entity.HasOne(d => d.Point).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.PointId)
                .HasConstraintName("shipments_point_id_fkey");

            entity.HasOne(d => d.Sclad).WithMany(p => p.Shipments)
                .HasForeignKey(d => d.ScladId)
                .HasConstraintName("shipments_sclad_id_fkey");
        });

        modelBuilder.Entity<ShipmentItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("shipment_items_pkey");

            entity.ToTable("shipment_items");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ShipmentId).HasColumnName("shipment_id");

            entity.HasOne(d => d.Product).WithMany(p => p.ShipmentItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("shipment_items_product_id_fkey");

            entity.HasOne(d => d.Shipment).WithMany(p => p.ShipmentItems)
                .HasForeignKey(d => d.ShipmentId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("shipment_items_shipment_id_fkey");
        });

        modelBuilder.Entity<Zalusky>(entity =>
        {
            entity.HasKey(e => e.ZaluskyId).HasName("zalusky_pkey");

            entity.ToTable("zalusky");

            entity.Property(e => e.ZaluskyId).HasColumnName("zalusky_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.ScladId).HasColumnName("sclad_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Zaluskies)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("zalusky_product_id_fkey");

            entity.HasOne(d => d.Sclad).WithMany(p => p.Zaluskies)
                .HasForeignKey(d => d.ScladId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("zalusky_sclad_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
