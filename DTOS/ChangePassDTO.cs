using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class ChangePassDTO 
    {
        public string? emailNv { get; set; }
        public string? newPassword { get; set; }

    }
}
