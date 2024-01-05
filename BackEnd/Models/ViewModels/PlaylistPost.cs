using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Showreel.Models.ViewModels;

public partial class PlaylistPost : Playlist
{
    public int[]? buildingsId { get; set; }
}
public partial class BuildingDetail
{
    public int? Id { get; set; }
    public string? BuildingName { get; set; }
}

public partial class PlaylistWithBuilding : Playlist
{
    public BuildingDetail[]? Buildings { get; set; }
}
