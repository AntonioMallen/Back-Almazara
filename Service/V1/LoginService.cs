using AutoMapper;
using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Repository.V1;
using Back_Almazara.Utility;

namespace Back_Almazara.Service.V1
{
    public class LoginService : ILoginService
    {

        private readonly IMapper _mapper;
        private readonly ILoginRepository _loginEntity;
        private readonly IHashUtility _hashUtility;
        public LoginService(IMapper mapper, ILoginRepository loginEntity, IHashUtility hashUtility)
        {
            _mapper = mapper;
            _loginEntity = loginEntity;
            _hashUtility = hashUtility;
        }

        public RepositoryResult<UsuarioDTO> Login(LoginDTO User, bool encrypt = true)
        {
            if(encrypt)
                User.PasswordNv = _hashUtility.HashPassword(User.PasswordNv);

            return _loginEntity.Login(User)
                .MapTo<TUser, UsuarioDTO>(_mapper);

        }

        public RepositoryResult<UsuarioDTO> Register(RegisterDTO User)
        {
            var tUser = _mapper.Map<TUser>(User);

            tUser.PasswordNv = _hashUtility.HashPassword(tUser.PasswordNv);

            return _loginEntity.Register(tUser)
                .MapTo<TUser, UsuarioDTO>(_mapper);
        }
        public RepositoryResult<UsuarioDTO> ChangePassword(ChangePassDTO user) {
            var tUser = _mapper.Map<TUser>(user);

            tUser.PasswordNv = _hashUtility.HashPassword(tUser.PasswordNv);

            return _loginEntity.ChangePassword(tUser)
                .MapTo<TUser, UsuarioDTO>(_mapper);
        }
    }
}
