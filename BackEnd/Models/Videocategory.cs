using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Videocategory
{
    public int Id { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Video> Videos { get; set; } = new List<Video>();
}
