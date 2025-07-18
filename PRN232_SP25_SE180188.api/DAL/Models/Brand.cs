﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DAL.Models;

public partial class Brand
{
    [Key]
    public int BrandId { get; set; }

    public string BrandName { get; set; } = null!;

    public string? Country { get; set; }

    public int? FoundedYear { get; set; }

    public string? Website { get; set; }

    [JsonIgnore]
    public virtual ICollection<Handbag> Handbags { get; set; } = new List<Handbag>();
}
