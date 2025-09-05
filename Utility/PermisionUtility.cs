using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using Back_Almazara.Service.V1;
using System.IdentityModel.Tokens.Jwt;

namespace Back_Almazara.Utility
{
    public class PermisionUtility
    {
        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
        public class AutorizeAttribute : Attribute, IAsyncAuthorizationFilter
        {
            private readonly int _nivelRequerido;

            public AutorizeAttribute(int nivelRequerido)
            {
                _nivelRequerido = nivelRequerido;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                // Omitir autorización para rutas de autenticación
                if (context.HttpContext.Request.Path.ToString().Contains("/api/v1/Auth/"))
                {
                    return;
                }
                if (_nivelRequerido != -1)
                {
                    var validationResult = await TokenValidationAsync(context.HttpContext);
                    if (validationResult != null)
                    {
                        context.Result = validationResult;
                        return;
                    }

                    // Verificar permisos de rol si se requiere un nivel específico

                    var user = context.HttpContext.User;
                    var roleClaim = user.Claims.FirstOrDefault(c => c.Type == "role");

                    if (roleClaim == null || !int.TryParse(roleClaim.Value, out int roleId) || roleId > _nivelRequerido)
                    {
                        context.Result = new ForbidResult();
                        return;
                    }
                }
            }

            private async Task<IActionResult?> TokenValidationAsync(HttpContext context)
            {
                var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
                string ip = context.Connection.RemoteIpAddress?.ToString() ?? "";

                if (string.IsNullOrEmpty(token))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token no válido");
                    return new UnauthorizedResult();
                }

                var tokenService = context.RequestServices.GetRequiredService<ITokenService>();
                var validation = tokenService.ValidateToken(token, ip);

                if (!string.IsNullOrEmpty(validation))
                {
                    return new NotFoundObjectResult(new { message = validation });
                }

                // Token válido - establecer claims
                try
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var claims = jwtToken.Claims.ToList();
                    var identity = new ClaimsIdentity(claims, "Bearer");
                    var principal = new ClaimsPrincipal(identity);
                    context.User = principal;

                    return null; // Autorización exitosa
                }
                catch
                {
                    return new UnauthorizedResult();
                }
            }
        }
    }
}
