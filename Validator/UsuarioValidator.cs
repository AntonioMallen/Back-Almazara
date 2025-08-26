using FluentValidation;
using Back_Almazara.DTOS;

namespace ProyectoJWT.Validator
{

    // Validador base genérico para AuthDTO
    public class AuthValidator<T> : AbstractValidator<T> where T : AuthDTO
    {
        public AuthValidator()
        {
            RuleFor(usuario => usuario).NotNull().WithMessage("Es necesario un usuario.");
            RuleFor(usuario => usuario.EmailNv).Length(0, 50).WithMessage("Email incorrecto.");
            RuleFor(usuario => usuario.EmailNv).EmailAddress().WithMessage("El formato del email no es valido.");
            RuleFor(usuario => usuario.PasswordNv).MinimumLength(8).WithMessage("La contraseña es muy corta");
            RuleFor(usuario => usuario.PasswordNv)
               .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula.")
               .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula.")
               .Matches(@"\d").WithMessage("La contraseña debe contener al menos un número.")
               .Matches(@"[!?\*\.]").WithMessage("La contraseña debe contener al menos uno de los siguientes caracteres especiales: ! ? * .");
        }
    }

    public class LoginValidator : AuthValidator<LoginDTO>
    {
        public LoginValidator()
        {
        }
    }

    public class RegisterValidator : AuthValidator<RegisterDTO>
    {
        public RegisterValidator()
        {
            RuleFor(usuario => usuario.NameNv).NotEmpty().NotNull().WithMessage("Es necesario un nombre de usuario.");
        }
    }

    public class UserValidator : AuthValidator<RegisterDTO>
    {
        public UserValidator()
        {
        }
    }

}
