using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Playlist
{
    public int Id { get; set; }

    public string? Status { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public string? Title { get; set; }

    public string? Duration { get; set; }
    public string? JsonPlaylist { get; set; }

    public string? Creator { get; set; }
}
