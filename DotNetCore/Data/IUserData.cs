using DotNetCore.Model;
using System.Collections.Generic;

namespace DotNetCore.Data
{
    public interface IUserData
    {
        List<User> Get();
        ResultMsg Save(User user);
    }
}
