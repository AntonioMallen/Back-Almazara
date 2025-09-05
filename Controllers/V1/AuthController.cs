using Asp.Versioning;
using Back_Almazara.DTOS;
using Back_Almazara.Service.V1;
using Back_Almazara.Utility;
using Back_Almazara.ViewModel;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using static Back_Almazara.Utility.PermisionUtility;

namespace Back_Almazara.Controllers.V1
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly ILoginService _service;
        private readonly ITokenService _tokenService;
        private readonly IHashUtility _hashUtility;
        private readonly IValidator<LoginDTO> _validatorLogin;
        private readonly IValidator<RegisterDTO> _validatorRegister;
        public AuthController(IValidator<LoginDTO> validatorLogin, IValidator<RegisterDTO> validatorRegister, ILoginService service, ITokenService tokenService, IHashUtility hashUtility) {
            _validatorLogin = validatorLogin;
            _validatorRegister = validatorRegister;
            _service = service;
            _tokenService = tokenService;
            _hashUtility = hashUtility;
        }



        [HttpPost("Login")]
        [Autorize(4)]
        public ActionResult Login(LoginDTO login)
        {
            //var result = _validatorLogin.Validate(login);
            //if (!result.IsValid)
            //{
            //    return BadRequest(result.Errors);
            //}

            var response = _service.Login(login);
            if (response.Success)
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";

                string token = _tokenService.GenerateToken(response.Data, ip);

                var loginData = new LoginViewModel()
                {
                    token = token,
                    userData = new LoginViewModel.UserData()
                    {
                        userID = _hashUtility.ToBase36(response.Data.IdI),
                        name = response.Data.NameNv,
                        role = _hashUtility.ToBase36(response.Data.RoleI),
                    }
                };
                return Ok(loginData);
            }

            return Unauthorized("Datos del usuario incorrectos.");
        }

        [HttpPost("Register")]
        [Autorize(4)]
        public ActionResult Register(RegisterDTO register)
        {
            var result = _validatorRegister.Validate(register);

            if (!result.IsValid)
            {
                return BadRequest(result.Errors);
            }
            var response = _service.Register(register);

            if (response.Success) // Despues de registrarnos vamos a generar el login
            {
                var ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "";
                string token = _tokenService.GenerateToken(response.Data, ip);

                var loginData = new LoginViewModel()
                {
                    token = token,
                    userData = new LoginViewModel.UserData()
                    {
                        userID = _hashUtility.ToBase36(response.Data.IdI),
                        name = response.Data.NameNv,
                        role = _hashUtility.ToBase36(response.Data.RoleI),
                    }
                };

                return Ok(loginData);
            }

            return Unauthorized(response.Message);
        }

        [HttpPost("ChangePassword")]
        [Autorize(-1)]
        public ActionResult ChangePassword(ChangePassDTO user)
        {
         
            var response = _service.ChangePassword(user);

            if (response.Success) // Despues de registrarnos vamos a generar el login
            {
                return Ok(user);
            }

            return BadRequest("No se ha podido cambiar la contraseña.");
        }

    }
}
