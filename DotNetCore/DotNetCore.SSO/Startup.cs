﻿using DotNetCore.Core.Services;
using DotNetCore.FrameWork.Utils;
using DotNetCore.SSO.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            //add sqlserver
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //add DI
            services.AddDependencyRegister();
            services.AddTfDI();

            services.AddCors();

            services.AddMvc()
                    //解决时间格式包含 T 字符
                    .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();

            //依赖注入扩展方法，实现简单的隐式依赖注入
            app.UseTfDI();

            //跨域，必须放在UseMvc()前
            app.UseCors(builder => builder.WithOrigins("http://localhost:5001")
                                          .AllowAnyHeader().AllowAnyMethod());

            app.UseMvc();
        }
    }
}