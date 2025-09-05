using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class TNoticesDetail
{
    public int IdI { get; set; }

    public int NoticeIdI { get; set; }

    public string? TitleNv { get; set; }

    public string? SubtitleNv { get; set; }

    public string? ContentNv { get; set; }

    public bool DisableB { get; set; }
}
