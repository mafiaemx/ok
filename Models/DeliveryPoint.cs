using System;
using System.Collections.Generic;

namespace ok.Models;

public partial class DeliveryPoint
{
    public int PointId { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Demand> Demands { get; set; } = new List<Demand>();

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}
