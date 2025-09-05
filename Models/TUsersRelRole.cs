using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class TUsersRelRole
{
    public int IdI { get; set; }

    public int UserIdI { get; set; }

    public int RoleIdI { get; set; }

    public bool DisableB { get; set; }
}
