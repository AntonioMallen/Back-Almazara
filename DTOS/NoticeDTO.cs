using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class NoticeDTO
    {
        public string IdI { get; set; }

        [Required]
        public string? NameNv { get; set; }

        [Required]
        public string? DescriptionNv { get; set; }

        [Required]
        public string? ImageNv { get; set; }

        public bool favorite { get; set; }

    }
}
