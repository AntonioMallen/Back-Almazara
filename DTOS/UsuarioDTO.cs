using System.ComponentModel.DataAnnotations;

namespace Back_Almazara.DTOS
{
    public class UsuarioDTO : AuthDTO
    {
        public int IdI { get; set; }
        public string NameNv { get; set; } = null!;
        public int RoleI { get; set; }

    }
}
