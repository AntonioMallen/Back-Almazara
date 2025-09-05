using Back_Almazara.DTOS;

namespace Back_Almazara.Service.V1
{
    public interface ITokenService
    {
        public string GenerateToken( UsuarioDTO usuario,string ipAddress);

        public string ValidateToken(string token,string ip);

    }
}
