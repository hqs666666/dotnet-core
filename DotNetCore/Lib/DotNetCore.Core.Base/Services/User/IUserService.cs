using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Domain.User;

namespace DotNetCore.Core.Base.Services.User
{
    public interface IUserService : ICoreService
    {
        List<UserProfile> GetList();
        Task<UserProfile> GetAsync(string userId);
        ResultMsg Register(RegisterDto dto);
        Task<ResultMsg> ValidUser(string userName, string password);
        IList<string> GetRole(string userId);
    }
}
