using DotNetCore.Core.Base.Services;
using DotNetCore.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Core.Services
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.User.User>().ToTable("SYS.USER");
            modelBuilder.Entity<UserProfile>().ToTable("SYS.USERPROFILE");
            modelBuilder.Entity<UserRole>().ToTable("SYS.USERROLE");
            modelBuilder.Entity<Role>().ToTable("SYS.ROLE");
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
    }
}
