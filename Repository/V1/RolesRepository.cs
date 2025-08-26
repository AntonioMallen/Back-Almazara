using System.Data;
using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public class RolesRepository : IRolesRepository
    {

        public RolesRepository()
        {
        }
        public RepositoryResult<List<RoleDTO>> Index()
        {
            using (var context = new AlmazaraContext())
            {

                var usersWithRoles = (from user in context.TUsers
                                         join role in context.TRoles on user.RoleI equals  role.IdI
                                         where role.IdI > 0 && role.DisableB == false && user.DisableB == false
                                         orderby user.NameNv descending
                                         select new RoleDTO
                                         {
                                             UserIdI = user.IdI,
                                             UserNameNv = user.NameNv,
                                             RoleIdI = role.IdI,
                                             RoleNameNv = role.NameNv
                                         }).ToList();

                return new RepositoryResult<List<RoleDTO>>() { Success = usersWithRoles != null && usersWithRoles.Count > 0, Data = usersWithRoles };
            }
        }
        public RepositoryResult<List<TRole>> ListRoles()
        {
            using (var context = new AlmazaraContext())
            {
                var roles = context.TRoles
                    .Where(rol => rol.DisableB == false)
                    .ToList();


                return new RepositoryResult<List<TRole>>() { Success = roles != null && roles.Count > 0, Data = roles };
            }
        }


        public RepositoryResult<bool> Modify(int user_id_i, int role_id_i)
        {
            using (var context = new AlmazaraContext())
            {
                

                var user = context.TUsers
                    .Where(usr => usr.IdI == user_id_i && usr.DisableB== false )
                    .FirstOrDefault();

                try
                {

                    if (user != null)
                    {
                        user.RoleI = role_id_i;
                        user.DisableB = false;

                        context.SaveChanges();
                    }
                    else 
                    {
                        return RepositoryResult<bool>.Fail($"No existe el usuario a editar.");
                    }


                    return RepositoryResult<bool>.Ok(true, "Se ha editado el comentario correctamente.");
                }
                catch (Exception ex)
                {
                    return RepositoryResult<bool>.Fail($"Error al editar el comentario: {ex.Message}");
                }

            }
        }

    }
}
