using System;
using System.Collections.Generic;

namespace ok.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public decimal Amount { get; set; }

    public string Unit { get; set; } = null!;

    public DateOnly? ExpirationDate { get; set; }

    public virtual ICollection<Demand> Demands { get; set; } = new List<Demand>();

    public virtual ICollection<ShipmentItem> ShipmentItems { get; set; } = new List<ShipmentItem>();

    public virtual ICollection<Zalusky> Zaluskies { get; set; } = new List<Zalusky>();
}
