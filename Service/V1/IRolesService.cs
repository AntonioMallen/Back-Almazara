using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;
using Back_Almazara.ViewModel;

namespace Back_Almazara.Service.V1
{
    public interface IRolesService
    {
        RepositoryResult<List<RoleResultDTO>> Index();
        RepositoryResult<List<RoleListDTO>> ListRoles();
        RepositoryResult<bool> Modify(string user_id_i, string role_id_i);
    }
}