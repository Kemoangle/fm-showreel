using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class BuildingRestriction
{
    public int Id { get; set; }

    public int? BuildingId { get; set; }

    public string? Type { get; set; }

    public string? Name { get; set; }

    public int? CategoryId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<RestrictionExcept> RestrictionExcepts { get; set; } = new List<RestrictionExcept>();
}
