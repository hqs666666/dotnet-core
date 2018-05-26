using System.Collections.Generic;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Domain.User;

namespace DotNetCore.Core.Base.Services.User
{
    public interface IUserService : ICoreService
    {
        List<UserProfile> Get();
        ResultMsg Register(RegisterDto dto);
        ResultMsg ValidUser(string userName, string password);
        IList<string> GetRole(string userId);
    }
}
