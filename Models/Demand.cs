using System;
using System.Collections.Generic;

namespace ok.Models;

public partial class Demand
{
    public int Id { get; set; }

    public int? PointId { get; set; }

    public int? ProductId { get; set; }

    public decimal RequiredQuantity { get; set; }

    public string PriorityLevel { get; set; } = null!;

    public decimal? CalculatedWeight { get; set; }

    public decimal? ForecastedStock { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual DeliveryPoint? Point { get; set; }

    public virtual Product? Product { get; set; }
}
