using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class RegisterDTO : AuthDTO
    {
        [Required]
        public string NameNv { get; set; } = null!;
    }
}
