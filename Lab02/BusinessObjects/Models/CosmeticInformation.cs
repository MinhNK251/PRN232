using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Models;

public partial class CosmeticInformation
{
    [Key]
    public string CosmeticId { get; set; } = null!;

    public string CosmeticName { get; set; } = null!;

    public string SkinType { get; set; } = null!;

    public string ExpirationDate { get; set; } = null!;

    public string CosmeticSize { get; set; } = null!;

    public decimal DollarPrice { get; set; }

    public string? CategoryId { get; set; }

    public virtual CosmeticCategory? Category { get; set; }
}
