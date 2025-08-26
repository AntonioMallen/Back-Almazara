using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class TFavorite
{
    public int IdI { get; set; }
    public int UserIdI { get; set; }
    public int NoticeIdI { get; set; }
    public bool DisableB { get; set; }
}
