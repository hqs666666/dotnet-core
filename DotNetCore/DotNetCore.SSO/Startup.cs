using System.IO;
using DotNetCore.Core.Services;
using DotNetCore.FrameWork.Middleware;
using DotNetCore.FrameWork.Utils;
using DotNetCore.SSO.Identity;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DotNetCore.SSO
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        //public static ILoggerRepository Repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //Repository = LogManager.CreateRepository("NETCoreRepository");
            //XmlConfigurator.Configure(Repository, new FileInfo("log4net.config"));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure identity server with in-memory stores, keys, clients and scopes
            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddInMemoryIdentityResources(ApiConfig.GetIdentityResourceResources())
                    .AddInMemoryApiResources(ApiConfig.GetApiResources())
                    .AddInMemoryClients(ApiConfig.GetClients())
                    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                    .AddProfileService<ProfileService>();

            //add mysql
            services.AddDbContext<DataContext>(options => options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            //add cors
            services.AddCors(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                });
            });

            //add DI
            services.AddDependencyRegister();
            services.AddTfDI();

            services.AddMvc()
                    //解决时间格式包含 T 字符
                    .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            app.UseCors("api");

            app.UseIdentityServer();

            app.UseMiddleware<ResponseMiddleware>();

            //依赖注入扩展方法，实现简单的隐式依赖注入
            app.UseTfDI();

            app.UseMvc();
        }
    }
}
