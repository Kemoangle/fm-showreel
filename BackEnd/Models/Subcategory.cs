using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Subcategory
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public virtual Category? Category { get; set; }
}
