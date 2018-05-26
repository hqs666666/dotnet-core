
using System;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Core.Base.Services
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
