using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Videocategory
{
    public int Id { get; set; }

    public int? VideoId { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Video? Video { get; set; }
}
