using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Restriction
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<BuildingRestriction> BuildingRestrictions { get; set; } = new List<BuildingRestriction>();

    public virtual Category? Category { get; set; }
}
