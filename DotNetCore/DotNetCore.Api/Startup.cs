using DotNetCore.Core.Services;
using DotNetCore.FrameWork.Filter;
using DotNetCore.FrameWork.Middleware;
using DotNetCore.FrameWork.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

            services.AddMvcCore()
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
                app.UseDatabaseErrorPage();
            }
          
            app.UseAuthentication();

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseTfDI();//依赖注入扩展方法

            app.UseCors("api");

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
