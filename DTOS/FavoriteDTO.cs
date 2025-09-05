using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class FavoriteDTO
    {
        [Required]
        public int user_id_i { get; set; } = -1;
     
        
        [Required]
        public int notice_id_i { get; set; } = -1;

    }
}
