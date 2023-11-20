using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Playlistvideo
{
    public int? VideoId { get; set; }

    public int? PlaylistId { get; set; }

    public virtual Playlist? Playlist { get; set; }

    public virtual Video? Video { get; set; }
}
