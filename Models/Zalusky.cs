using System;
using System.Collections.Generic;

namespace ok.Models;

public partial class Zalusky
{
    public int ZaluskyId { get; set; }

    public int? ScladId { get; set; }

    public int? ProductId { get; set; }

    public decimal Quantity { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Sclad? Sclad { get; set; }
}
