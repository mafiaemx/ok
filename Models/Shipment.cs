using System;
using System.Collections.Generic;

namespace ok.Models;

public partial class Shipment
{
    public int Id { get; set; }

    public int? ScladId { get; set; }

    public int? PointId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual DeliveryPoint? Point { get; set; }

    public virtual Sclad? Sclad { get; set; }

    public virtual ICollection<ShipmentItem> ShipmentItems { get; set; } = new List<ShipmentItem>();
}
