using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Videolist
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Remark { get; set; }

    public DateTime? CreatedTime { get; set; }

    public DateTime? LastUpdatedTime { get; set; }

    public virtual ICollection<VideoVideolist> VideoVideolists { get; set; } = new List<VideoVideolist>();
}
