using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class AuthDTO
    {
        [Required]
        public string EmailNv { get; set; } = null!;

        [Required]
        public string PasswordNv { get; set; } = null!;
    }
}
