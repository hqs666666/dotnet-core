using System;
using DotNetCore.FrameWork.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.FrameWork.Helpers
{
    public class CookieHelper
    {
        public static void SetCookie<T>(string key,T value,int minutes = 30)
        {
            var lValue = JsonHelper.ObjectToJson(value);
            HttpContext.Response.Cookies.Append(key, lValue, new CookieOptions
            {
                Expires = DateTime.Now.AddMinutes(minutes)
            });
        }

        public static T GetCookie<T>(string key)
        {
            var lExist = HttpContext.Request.Cookies.TryGetValue(key, out string lValue);
            if (!lExist)
                return default(T);

            return JsonHelper.JsonToObject<T>(lValue);
        }

        public static void RemoveCookie(string key)
        {
            HttpContext.Response.Cookies.Delete(key);
        }

        public static void SetSession<T>(string key, T value)
        {
            var lValue = JsonHelper.ObjectToBytes(value);
            HttpContext.Session.Set(key,lValue);
        }

        public static T GetSession<T>(string key)
        {
            var lExist = HttpContext.Session.TryGetValue(key, out byte[] lValue);
            if (!lExist)
                return default(T);

            return JsonHelper.BytesToObject<T>(lValue);
        }

        private static readonly IHttpContextAccessor _httpContextAccessor =
            DI.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

        private static HttpContext HttpContext => _httpContextAccessor.HttpContext;
    }
}
