using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Landlordad
{
    public int Id { get; set; }

    public int? Loop { get; set; }

    public int? VideoId { get; set; }

    public int? BuildingId { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Video? Video { get; set; }
}
