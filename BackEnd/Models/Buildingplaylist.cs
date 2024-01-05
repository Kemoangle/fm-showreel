using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Buildingplaylist
{
    public int Id { get; set; }

    public int? BuildingId { get; set; }

    public int? PlaylistId { get; set; }

    public virtual Building? Building { get; set; }

    public virtual Playlist? Playlist { get; set; }
}
