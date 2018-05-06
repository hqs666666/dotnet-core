using DotNetCore.Model;
using DotNetCore.Uitl;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCore.Data
{
    public class UserData : CoreData, IUserData
    {
        private readonly DbSet<User> UserDbSet;
        public UserData(EFCoreContext context) : base(context)
        {
            UserDbSet = context.Set<User>();
        }

        public List<User> Get()
        {
            return UserDbSet.ToList();
        }

        public ResultMsg Save(User user)
        {
            UserDbSet.Add(user);
            return SaveChanges();
        }
    }
}
