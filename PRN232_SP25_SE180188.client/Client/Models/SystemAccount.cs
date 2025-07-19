using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Client.Models;

public partial class SystemAccount
{
    [Key]
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int? Role { get; set; }

    public bool? IsActive { get; set; }
}
