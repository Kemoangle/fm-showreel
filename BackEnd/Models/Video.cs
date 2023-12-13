﻿using System;
using System.Collections.Generic;

namespace Showreel.Models;

public partial class Video
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? FilePath { get; set; }

    public int? Duration { get; set; }

    public string? KeyNo { get; set; }

    public string? Remark { get; set; }

    public DateOnly? CreateTime { get; set; }

    public DateOnly? LastUpdateTime { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<Landlordad> Landlordads { get; set; } = new List<Landlordad>();

    public virtual ICollection<Rule> Rules { get; set; } = new List<Rule>();

    public virtual ICollection<VideoVideolist> VideoVideolists { get; set; } = new List<VideoVideolist>();

    public virtual ICollection<Videocategory> Videocategories { get; set; } = new List<Videocategory>();
}
