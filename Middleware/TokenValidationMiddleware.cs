using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using Back_Almazara.Service.V1;
using System.Security.Claims;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _neusuariot;

    public TokenValidationMiddleware(RequestDelegate neusuariot)
    {
        _neusuariot = neusuariot;
    }

    public async Task Invoke(HttpContext context)
    {
        // Authentication
        if (context.Request.Path.StartsWithSegments("/api/v1/Auth")) // Login & Register    || noAuthNeed.Contains(context.Request.Path.ToString())
        {
            await _neusuariot(context);
            return;
        }

        // Token validation
        var token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();
        string ip = context.Connection.RemoteIpAddress?.ToString() ?? "";

        if (!string.IsNullOrEmpty(token))
        {
            var tokenService = context.RequestServices.GetRequiredService<ITokenService>();

            var validation = tokenService.ValidateToken(token, ip);
            if (string.IsNullOrEmpty(validation))
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);

                var claims = jwtToken.Claims.ToList();

                var identity = new ClaimsIdentity(claims, "Bearer");
                var principal = new ClaimsPrincipal(identity);

                context.User = principal;

                await _neusuariot(context);
                return;
            }
            else
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync(validation);
            }
        }

        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Token no válido");

    }



}