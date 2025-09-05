using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class NoticeEditDTO
    {
        public int IdI { get; set; }

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
