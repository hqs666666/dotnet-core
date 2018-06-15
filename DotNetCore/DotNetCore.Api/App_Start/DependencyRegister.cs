using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.Cache;
using DotNetCore.Core.Base.Services.Log;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Core.Services;
using DotNetCore.Core.Services.Cache;
using DotNetCore.Core.Services.Log;
using DotNetCore.Core.Services.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Api
{
    public static class Dependency
    {
        public static IServiceCollection AddDependencyRegister(this IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IWorkContext, WorkContext>();
            services.AddTransient<IDataContext, DataContext>();
            services.AddTransient<IConfigService, ConfigService>();
            services.AddTransient<ICoreService, CoreService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPicResourceService, PicResourceService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IRedisService, RedisService>();

            return services;
        }
    }
}
