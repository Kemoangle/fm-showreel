using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class RestrictionExcept
{
    public int Id { get; set; }

    public int? BuildingRestrictionId { get; set; }

    public int? VideoTypeId { get; set; }

    public virtual BuildingRestriction? BuildingRestriction { get; set; }

    public virtual VideoType? VideoType { get; set; }
}
