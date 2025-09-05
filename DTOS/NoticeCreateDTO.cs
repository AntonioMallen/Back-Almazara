using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class NoticeCreateDTO
    {
        public string? NameNv { get; set; }

        public string? DescriptionNv { get; set; }

        [Required]
        public string? ImageNv { get; set; }

    }
}
