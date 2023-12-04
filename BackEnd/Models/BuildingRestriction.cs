using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class BuildingRestriction
{
    public int Id { get; set; }

    public int? BuildingId { get; set; }

    public int? RestrictionId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Restriction? Restriction { get; set; }

    public virtual ICollection<RestrictionExcept> RestrictionExcepts { get; set; } = new List<RestrictionExcept>();
}
