using System;
using System.Collections.Generic;

namespace ok.Models;

public partial class ShipmentItem
{
    public int Id { get; set; }

    public int? ShipmentId { get; set; }

    public int? ProductId { get; set; }

    public decimal Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Shipment? Shipment { get; set; }
}
