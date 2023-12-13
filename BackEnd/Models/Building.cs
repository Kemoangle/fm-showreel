using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Building
{
    public int Id { get; set; }

    public string? BuildingName { get; set; }

    public string? Address { get; set; }

    public string? District { get; set; }

    public string? Remark { get; set; }

    public DateOnly? CreateTime { get; set; }

    public DateOnly? LastUpdateTime { get; set; }

    public string? Zone { get; set; }

    public int? PostalCode { get; set; }

    public virtual ICollection<Landlordad> Landlordads { get; set; } = new List<Landlordad>();

    public virtual ICollection<Restriction> Restrictions { get; set; } = new List<Restriction>();

    public virtual ICollection<Rule> Rules { get; set; } = new List<Rule>();
}
