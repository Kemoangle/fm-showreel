using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class VideoType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RestrictionExcept> RestrictionExcepts { get; set; } = new List<RestrictionExcept>();

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
