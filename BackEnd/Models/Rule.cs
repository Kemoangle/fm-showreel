using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Rule
{
    public int Id { get; set; }

    public int? VideoId { get; set; }

    public int? NoBackToBack { get; set; }

    public int? DoNotPlay { get; set; }

    public virtual Building? DoNotPlayNavigation { get; set; }

    public virtual Category? NoBackToBackNavigation { get; set; }

    public virtual Video? Video { get; set; }
}
