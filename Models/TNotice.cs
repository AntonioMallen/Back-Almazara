using System;
using System.Collections.Generic;

namespace Back_Almazara.Models;

public partial class TNotice
{
    public int IdI { get; set; }

    public string? NameNv { get; set; }

    public string? DescriptionNv { get; set; }

    public bool DisableB { get; set; }

    public byte[]? ImageNv { get; set; }
}
