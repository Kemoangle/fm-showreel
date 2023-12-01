using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Restriction
{
    public int Id { get; set; }

    public int? BuildingId { get; set; }

    public int? VideoId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Video? Video { get; set; }
}
