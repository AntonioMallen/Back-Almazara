using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Service.V1
{
    public interface ILoginService
    {
        RepositoryResult<UsuarioDTO> Login(LoginDTO User, bool encrypt = true);
        RepositoryResult<UsuarioDTO> Register(RegisterDTO User);
        RepositoryResult<UsuarioDTO> ChangePassword(ChangePassDTO user);
    }
}
