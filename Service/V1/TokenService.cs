using Back_Almazara.DTOS;
using Back_Almazara.Repository.V1;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Back_Almazara.Service.V1
{
    public class TokenService : ITokenService
    {

        private readonly IConfiguration _config;
        private readonly ILoginService _service;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration configuration, ILoginService service)
        {
            _config = configuration;
           IConfigurationSection seccion = _config.GetSection("JwtSettings");
           _secretKey = seccion.GetValue("SecretKey", "") ?? "";
            _issuer = seccion.GetValue("ValidIssuer", "") ?? "";
            _audience = seccion.GetValue("ValidAudience", "") ?? "";
            _service = service;
        }



        /// <summary>
        /// Genera un token de acceso JWT
        /// </summary>
        /// <param name="claims">Informacion pasada a traves del Token</param>
        /// <returns></returns>
        public string GenerateToken(UsuarioDTO usuario, string ip)
        {
            string secretKey = _secretKey;
            string issuer = _issuer;
            string auddience = _audience;

            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            Claim[] claims = {
                new Claim("id"          , usuario.IdI.ToString()),
                new Claim("email"       , usuario.EmailNv),
                new Claim("password"    , usuario.PasswordNv),
                new Claim("role"        , usuario.RoleI.ToString()),
                new Claim("IP"          , ip)
            };


            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: auddience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ValidateToken(string token, string ip)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var email = jwtToken.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "";
            var password = jwtToken.Claims.FirstOrDefault(c => c.Type == "password")?.Value ?? "";
            var ipClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "IP")?.Value ?? "";

            if (! ValidarIP(ipClaim, ip))
            {
                return "Acceso unautorizado, intente de nuevo.";
            }

            var result = _service.Login(new LoginDTO() { EmailNv = email, PasswordNv = password }, false);
            if (result.Success) {
                return "";
            }

            return result.Message ?? "Ha ocurrido un error validando la identidad.";
        }

        private bool ValidarIP(string ipClaim, string ip)
        {

            if (!string.IsNullOrEmpty(ipClaim))
            {

                if (ipClaim == ip)
                {

                    return true;
                }
            }
            return false;
        }




    }
}
