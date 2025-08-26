using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.ViewModel
{
    public class NoticeDetailViewModel
    {

        public string? IdI { get; set; }

        [Required]
        public string? NameNv { get; set; }

        public string? DescriptionNv { get; set; }

        [Required]
        public string? ImageNv { get; set; }

        public string? TitleNv { get; set; }

        public string? SubtitleNv { get; set; }

        public string? ContentNv { get; set; }
    }
}
