using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects.Models;

public partial class CosmeticCategory
{
    [Key]
    public string CategoryId { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string UsagePurpose { get; set; } = null!;

    public string FormulationType { get; set; } = null!;

    public virtual ICollection<CosmeticInformation> CosmeticInformations { get; set; } = new List<CosmeticInformation>();
}
