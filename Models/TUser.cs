using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class TUser
{
    public int IdI { get; set; }

    public string NameNv { get; set; } = null!;

    public string EmailNv { get; set; } = null!;

    public string PasswordNv { get; set; } = null!;

    public int RoleI { get; set; }

    public bool DisableB { get; set; }
}
