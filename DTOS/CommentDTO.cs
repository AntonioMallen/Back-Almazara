using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class CommentDTO
{
    public int IdI { get; set; }

    public int UserIdI { get; set; }
    public string? userNameNv { get; set; }

    public int NoticeIdI { get; set; }

    public string? ContentNv { get; set; }

    public DateTime? DateDt { get; set; }

    public bool DisableB { get; set; }
}
