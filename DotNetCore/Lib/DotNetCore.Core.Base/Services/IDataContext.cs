
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Core.Base.Services
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
