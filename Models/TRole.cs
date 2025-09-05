using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class TRole
{
    public int IdI { get; set; }

    public string NameNv { get; set; } = null!;

    public int PermissionIdI { get; set; }

    public bool DisableB { get; set; }
}
