using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class VideoVideolist
{
    public int Id { get; set; }

    public int? VideoId { get; set; }

    public int? VideoListId { get; set; }

    public int? LoopNum { get; set; }

    public virtual Video? Video { get; set; }

    public virtual Videolist? VideoList { get; set; }
}
