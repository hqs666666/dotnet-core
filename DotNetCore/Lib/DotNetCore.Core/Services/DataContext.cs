using System.Threading.Tasks;
using DotNetCore.Core.Base.Services;
using DotNetCore.Domain.File;
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
            modelBuilder.Entity<Domain.User.User>().ToTable("sys_user");
            modelBuilder.Entity<UserProfile>().ToTable("sys_userprofile");
            modelBuilder.Entity<UserRole>().ToTable("sys_user_role");
            modelBuilder.Entity<Role>().ToTable("sys_role");
            modelBuilder.Entity<PicRelate>().ToTable("pic_relate");
            modelBuilder.Entity<PicResource>().ToTable("pic_resource");
        }

        public async Task<int> SaveChangesAsync()
        {
           return await base.SaveChangesAsync();
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
