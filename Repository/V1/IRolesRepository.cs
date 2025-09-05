using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public interface IRolesRepository
    {
        RepositoryResult<List<RoleDTO>> Index();
        RepositoryResult<List<TRole>> ListRoles();
        RepositoryResult<bool> Modify(int user_id_i, int role_id_i);

    }
}
