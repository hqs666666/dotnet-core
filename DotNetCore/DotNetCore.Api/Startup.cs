using System.IO;
using DotNetCore.Api.App_Start;
using DotNetCore.Core.Services;
using DotNetCore.FrameWork.Filter;
using DotNetCore.FrameWork.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace DotNetCore.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                    .AddIdentityServerAuthentication(options =>
                    {
                        options.Authority = AppSettings.Configuration["Host:SSO"];
                        options.RequireHttpsMetadata = false;   //不需要https    
                        options.ApiName = "api";    //api的name，需要和config的名称相同
                    });

            //add sqlserver
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

            //添加授权过滤器
            services.AddMvcCore(option => { option.Filters.Add(typeof(CustomerAuthorizationFilter)); })
                    .AddAuthorization()
                    .AddJsonOptions(options => options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss");

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseAuthentication();

            app.UseTfDI();//依赖注入扩展方法

            app.UseCors("api");

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Static/Files")),
            //    RequestPath = new PathString("/src")
            //});

            app.UseMvc();
        }
    }
}
