using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class CommentViewModel
{
    public string? IdI { get; set; }

    public string? UserIdI { get; set; }
    public string? userNameNv { get; set; }
    public string? NoticeIdI { get; set; }

    public string? ContentNv { get; set; }

    public DateTime? DateDt { get; set; }

}
