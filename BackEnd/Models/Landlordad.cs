using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Landlordad
{
    public int Id { get; set; }

    public int? Loop { get; set; }

    public int? VideoId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int? BuildingId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Video? Video { get; set; }
}
