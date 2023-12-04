using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Restriction> Restrictions { get; set; } = new List<Restriction>();

    public virtual ICollection<Videocategory> Videocategories { get; set; } = new List<Videocategory>();
}
