using Asp.Versioning;
using Back_Almazara.DTOS;
using Back_Almazara.Service.V1;
using Back_Almazara.ViewModel;
using Microsoft.AspNetCore.Mvc;
using static Back_Almazara.Utility.PermisionUtility;

namespace Back_Almazara.Controllers.V1
{
    [ApiVersion(1.0)]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }


        [ApiVersion(1.0)]
        [HttpPost("RememberMe")]
        [Autorize(-1)]
        public async Task<IActionResult> RememberMe(RememberMeDTO content)
        {

            if (_emailService.UserExists(content.email))
            {
                var body = $"<!DOCTYPE html>\r\n<html lang=\"es\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Recuperación de Contraseña</title>\r\n  <style>\r\n    body {{\r\n      font-family: 'Segoe UI', sans-serif;\r\n      background-color: #F9F6F1;\r\n      margin: 0;\r\n      padding: 0;\r\n      color: #5C4B3F;\r\n    }}\r\n    .container {{\r\n      max-width: 600px;\r\n      margin: 2rem auto;\r\n      background-color: #FFFFFF;\r\n      border-radius: 15px;\r\n      box-shadow: 5px 5px 20px rgba(39, 39, 39, 0.1);\r\n      padding: 2.5rem;\r\n      text-align: center;\r\n      border: 1px solid #E4D8C9;\r\n    }}\r\n    h1 {{\r\n      color: #3E2D23;\r\n      font-size: 2rem;\r\n      margin-bottom: 1rem;\r\n    }}\r\n    .artisan-border {{\r\n      width: 80%;\r\n      height: 4px;\r\n      background: linear-gradient(90deg, #D4A55E 0%, #8A9B6E 100%);\r\n      margin: 1.5rem auto;\r\n      border-radius: 2px;\r\n    }}\r\n    p {{\r\n      font-size: 1rem;\r\n      line-height: 1.6;\r\n      margin-bottom: 1.2rem;\r\n    }}\r\n    .code-box {{\r\n      display: inline-block;\r\n      background-color: #FFF8F0;\r\n      border: 2px dashed #8A9B6E;\r\n      color: #3E2D23;\r\n      padding: 1rem 2rem;\r\n      font-size: 1.5rem;\r\n      font-weight: bold;\r\n      letter-spacing: 3px;\r\n      border-radius: 10px;\r\n      margin-top: 1rem;\r\n      margin-bottom: 2rem;\r\n    }}\r\n    .button {{\r\n      display: inline-block;\r\n      padding: 0.8rem 2rem;\r\n      background-color: #8A9B6E;\r\n      color: #FFF8F0 !important;\r\n      text-decoration: none;\r\n      border-radius: 25px;\r\n      font-weight: 500;\r\n      transition: all 0.3s ease;\r\n      border: 2px solid transparent;\r\n    }}\r\n    .button:hover {{\r\n      background-color: #D4A55E;\r\n    }}\r\n    footer {{\r\n      margin-top: 2rem;\r\n      font-size: 0.85rem;\r\n      color: #8A9B6E;\r\n    }}\r\n  </style>\r\n</head>\r\n<body>\r\n  <div class=\"container\">\r\n    <h1>¿Olvidaste tu contraseña?</h1>\r\n    <div class=\"artisan-border\"></div>\r\n    <p>Hemos recibido una solicitud para restablecer tu contraseña en nuestro blog sobre productos artesanales y tradiciones rurales.</p>\r\n    <p>Usa el siguiente código para recuperar el acceso a tu cuenta:</p>\r\n    <div class=\"code-box\">{content.codeNv}</div>\r\n    <p>Si no solicitaste este cambio, puedes ignorar este mensaje. </p>\r\n    <a class=\"button\" href=\"https://Almazara.mallengimeno.dev/login/rememberMe\">Ir a la página de recuperación</a>\r\n    <footer>\r\n      © 2025 Almazara – Todos los derechos reservados\r\n    </footer>\r\n  </div>\r\n</body>\r\n</html>";

                await _emailService.SendEmailAsync(content.email, "Recuperar contraseña", body, true);
                return Ok("Correo de recuperación enviado.");

            }
            return NotFound("No se ha encontrado el correo introducido.");

        }

        [ApiVersion(1.0)]
        [HttpPost("SupportEmail")]
        [Autorize(4)]
        public async Task<IActionResult> SupportEmail(EmailViewModal content)
        {

            var email = _emailService.EmailUser(content.userID ?? "");
            if (email != "")
            {
                var body = $"<!DOCTYPE html>\r\n<html lang=\"es\">\r\n<head>\r\n  <meta charset=\"UTF-8\">\r\n  <title>Nuevo mensaje de contacto</title>\r\n  <style>\r\n    body {{\r\n      font-family: 'Segoe UI', sans-serif;\r\n      background-color: #F9F6F1;\r\n      margin: 0;\r\n      padding: 0;\r\n      color: #5C4B3F;\r\n    }}\r\n    .container {{\r\n      max-width: 600px;\r\n      margin: 2rem auto;\r\n      background-color: #FFFFFF;\r\n      border-radius: 15px;\r\n      box-shadow: 5px 5px 20px rgba(39, 39, 39, 0.1);\r\n      padding: 2.5rem;\r\n      border: 1px solid #E4D8C9;\r\n    }}\r\n    h1 {{\r\n      color: #3E2D23;\r\n      font-size: 2rem;\r\n      margin-bottom: 1rem;\r\n      text-align: center;\r\n    }}\r\n    .artisan-border {{\r\n      width: 80%;\r\n      height: 4px;\r\n      background: linear-gradient(90deg, #D4A55E 0%, #8A9B6E 100%);\r\n      margin: 1.5rem auto;\r\n      border-radius: 2px;\r\n    }}\r\n    .info {{\r\n      font-size: 1rem;\r\n      margin-bottom: 1rem;\r\n      background-color: #FFF8F0;\r\n      padding: 1rem;\r\n      border-left: 4px solid #8A9B6E;\r\n      border-radius: 5px;\r\n    }}\r\n    .label {{\r\n      font-weight: bold;\r\n      color: #3E2D23;\r\n    }}\r\n    .content-box {{\r\n      margin-top: 1.5rem;\r\n      padding: 1rem;\r\n      background-color: #F9F6F1;\r\n      border: 1px dashed #8A9B6E;\r\n      border-radius: 10px;\r\n      white-space: pre-line;\r\n    }}\r\n    footer {{\r\n      margin-top: 2rem;\r\n      font-size: 0.85rem;\r\n      color: #8A9B6E;\r\n      text-align: center;\r\n    }}\r\n  </style>\r\n</head>\r\n<body>\r\n  <div class=\"container\">\r\n    <h1>Nuevo mensaje de contacto</h1>\r\n    <div class=\"artisan-border\"></div>\r\n    \r\n    <div class=\"info\">\r\n      <div><span class=\"label\">Correo del usuario:</span> {email}</div>\r\n      <div><span class=\"label\">Asunto:</span> {content.Subject}</div>\r\n    </div>\r\n\r\n    <div class=\"content-box\">\r\n      {content.Content}\r\n    </div>\r\n\r\n    <footer>\r\n      Mensaje automático generado desde el formulario de contacto de Almazara.\r\n    </footer>\r\n  </div>\r\n</body>\r\n</html>\r\n";
               
                await _emailService.SendEmailAsync("almazara.oficial@gmail.com", "Soporte", body, true);
                return Ok("Correo enviado");
            }

            return NotFound("No existe el usuario o correo.");
        }
    }
}
