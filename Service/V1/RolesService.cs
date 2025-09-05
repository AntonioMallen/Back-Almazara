using System.Buffers.Text;
using System.Xml.Linq;
using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Repository.V1;
using Back_Almazara.Utility;
using Back_Almazara.ViewModel;

namespace Back_Almazara.Service.V1
{
    public class RolesService : IRolesService
    {

        private readonly IMapper _mapper;
        private readonly IRolesRepository _rolesEntity;
        private readonly IHashUtility _hashUtility;
        public RolesService(IMapper mapper, IRolesRepository rolesEntity, IHashUtility hashUtility)
        {
            _mapper = mapper;
            _rolesEntity = rolesEntity;
            _hashUtility = hashUtility;
        }

        public RepositoryResult<List<RoleResultDTO>> Index()
        {
            RepositoryResult<List<RoleDTO>> notices = _rolesEntity.Index();

            if (notices.Success)
            {
                var roles = notices.Data?
                    .Select(r => {
                        return new RoleResultDTO
                        {
                            UserIdI = _hashUtility.ToBase36(r.UserIdI),
                            UserNameNv = r.UserNameNv,
                            RoleIdI = _hashUtility.ToBase36(r.RoleIdI),
                            RoleNameNv = r.RoleNameNv,
                        };
                    })
                    .ToList();

                return RepositoryResult<List<RoleResultDTO>>.Ok(roles);
            }
            if (notices.Data?.Count == 0)
            {
                return RepositoryResult<List<RoleResultDTO>>.Ok(new List<RoleResultDTO>(), "No hay roles");
            }
            return RepositoryResult<List<RoleResultDTO>>.Fail("Ha ocurrido un problema");
        }

        public RepositoryResult<List<RoleListDTO>> ListRoles()
        {

            var listRoles = _rolesEntity.ListRoles();

            if (listRoles.Success)
            {
                var roles = listRoles.Data?
                    .Select(r => {
                        return new RoleListDTO
                        {
                            RoleIdI = _hashUtility.ToBase36(r.IdI)
                            , RoleNameNv = r.NameNv
                        };
                    })
                    .ToList();

                return RepositoryResult<List<RoleListDTO>>.Ok(roles);
            }

            if (listRoles.Data?.Count == 0)
            {
                return RepositoryResult<List<RoleListDTO>>.Ok(new List<RoleListDTO>(), "No hay roles");
            }

            return RepositoryResult<List<RoleListDTO>>.Fail("Ha ocurrido un problema");
        }

        public RepositoryResult<bool> Modify(string user_id_i, string role_id_i)
        {
            var userID = (int)_hashUtility.FromBase36(user_id_i ?? "");
            var roleID = (int)_hashUtility.FromBase36(role_id_i ?? "");

            return _rolesEntity.Modify(userID, roleID);
        }


    }
}
