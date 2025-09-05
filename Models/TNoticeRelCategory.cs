using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class TNoticeRelCategory
{
    public int IdI { get; set; }

    public int NoticeIdI { get; set; }

    public int CategoryIdI { get; set; }

    public bool DisableB { get; set; }
}
