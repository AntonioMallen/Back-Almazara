using Back_Almazara.DTOS;
using Back_Almazara.Models;
using Back_Almazara.Utility;

namespace Back_Almazara.Repository.V1
{
    public interface ILoginRepository
    {
        RepositoryResult<TUser> Login(LoginDTO User);
        RepositoryResult<TUser> Register(TUser User);
        RepositoryResult<TUser> ChangePassword(TUser user);
        RepositoryResult<TUser> ExistsUser(string email, int? userID = null);
    }
}
