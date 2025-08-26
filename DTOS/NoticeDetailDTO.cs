using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class NoticeDetailDTO : NoticeDTO
    {
        public int NoticeIdI { get; set; }

        public string? TitleNv { get; set; }

        public string? SubtitleNv { get; set; }

        public string? ContentNv { get; set; }
    }
}
