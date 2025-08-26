using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public class LoginRepository : ILoginRepository
    {




        public RepositoryResult<TUser> Login(LoginDTO user)
        {
            using (var context = new AlmazaraContext())
            {
                var usuario = context.TUsers
                    .FirstOrDefault(u => ( u.EmailNv == user.EmailNv || u.NameNv == user.EmailNv) && u.PasswordNv == user.PasswordNv && u.DisableB == false);
                return new RepositoryResult<TUser>() { Success = usuario != null, Data = usuario };
            }
        }

        public RepositoryResult<TUser> Register(TUser user)
        {
            using (var context = new AlmazaraContext())
            {

                var existingUser = context.TUsers
                .FirstOrDefault(u => u.EmailNv == user.EmailNv && u.DisableB == false);

                if (existingUser != null)
                {
                    // Usuario ya existe
                    return RepositoryResult<TUser>.Fail("Ya existe un usuario con esas credenciales"); 
                }

                // Agregar nuevo usuario
                context.TUsers.Add(user);
                context.SaveChanges();

                return RepositoryResult<TUser>.Ok(user, "Registro realizado con exito."); 
            }
        }
        public RepositoryResult<TUser> ChangePassword(TUser user)
        {
            using (var context = new AlmazaraContext())
            {

                var existingUser = context.TUsers
                .FirstOrDefault(u => u.EmailNv == user.EmailNv && u.DisableB == false);

                if (existingUser == null)
                {
                    // Usuario ya existe
                    return RepositoryResult<TUser>.Fail("No existe el usuario o esta deshabilitado.");
                }

                existingUser.PasswordNv = user.PasswordNv;
                context.SaveChanges();

                return RepositoryResult<TUser>.Ok(existingUser, "Se ha modificado la contraseña.");
            }
        }

        public RepositoryResult<TUser> ExistsUser(string email, int? userID = null)
        {
            using (var context = new AlmazaraContext())
            {
                TUser? usuario = null;
                if (userID != null)
                {
                    usuario = context.TUsers
                        .FirstOrDefault(u => u.IdI == userID  && u.DisableB == false);
                }
                else
                {
                    usuario = context.TUsers
                        .FirstOrDefault(u => u.EmailNv == email  && u.DisableB == false);
                }
                return new RepositoryResult<TUser>() { Success = usuario != null, Data = usuario };
            }
        }
    }
}
