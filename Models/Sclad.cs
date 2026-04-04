using System;
using System.Collections.Generic;

namespace ok.Models;

public partial class Sclad
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public virtual ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    public virtual ICollection<Zalusky> Zaluskies { get; set; } = new List<Zalusky>();
}
