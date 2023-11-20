using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Buildingrestriction
{
    public int? BuildingId { get; set; }

    public int? RestrictionId { get; set; }

    public bool? IsActive { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Restriction? Restriction { get; set; }
}
