using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DotNetCore.Core.Services
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            IConfigurationRoot lConfiguration = new ConfigurationBuilder()
                                               .SetBasePath(Directory.GetCurrentDirectory())
                                               .AddJsonFile("appsettings.json")
                                               .Build();
            var lBuilder = new DbContextOptionsBuilder<DataContext>();
            var lConnectionString = lConfiguration.GetConnectionString("DefaultConnection");
            lBuilder.UseMySql(lConnectionString);
            return new DataContext(lBuilder.Options);
        }

    }
}
