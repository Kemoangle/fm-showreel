using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Playlist
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string? Title { get; set; }

    public string? JsonPlaylist { get; set; }

    public string? JsonListVideo { get; set; }

    public string? Creator { get; set; }

    public int? ParentId { get; set; }
}
