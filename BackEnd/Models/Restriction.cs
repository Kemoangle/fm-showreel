using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Restriction
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public int? BuildingId { get; set; }

    public string? Type { get; set; }

    public string? ArrCategory { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Category? Category { get; set; }
}
