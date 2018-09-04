using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.FrameWork.Utils
{
    public class DI
    {
        public static IServiceCollection Services { get; set; }

        public static IServiceProvider ServiceProvider { get; set; }
    }

    public static class Extensions
    {
        public static IServiceCollection AddTfDI(this IServiceCollection services)
        {
            DI.Services = services;

            return services;
        }

        public static IApplicationBuilder UseTfDI(this IApplicationBuilder builder)
        {
            DI.ServiceProvider = builder.ApplicationServices;

            return builder;
        }

    }
}
