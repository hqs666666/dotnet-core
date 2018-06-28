using System;
using DotNetCore.Core.Services;
using DotNetCore.FrameWork.Filter;
using DotNetCore.FrameWork.Middleware;
using DotNetCore.FrameWork.Utils;
using DotNetCore.SSO.Identity;
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
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
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

            services.AddMvc(options =>
                    {
                        //全局异常过滤器 与 ExceptionMiddleware 二选一
                        options.Filters.Add<ExceptionFilter>();
                        options.Filters.Add<CustomerAuthorizationFilter>();
                    })
                    //解决时间格式包含 T 字符
                    .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");

            //添加Session 服务
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
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

            //异常处理中间件
            //app.UseMiddleware<ExceptionMiddleware>();

            //依赖注入扩展方法，实现简单的隐式依赖注入
            app.UseTfDI();

            //启用Session
            app.UseSession();
            app.UseMvc(options =>
            {
                options.MapRoute(
                    name: "default",
                    template: "{controller=Values}/{action=Index}/{id?}");
            });
        }
    }
}
