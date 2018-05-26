using System.Linq;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.Core.Services
{
    public class WorkContext : IWorkContext
    {
        private bool mDisposed;

        private readonly IHttpContextAccessor mAccessor;

        public WorkContext(IHttpContextAccessor accessor)
        {
            mAccessor = accessor;
        }

        public void Dispose()
        {
            if (mDisposed) return;

            //if (disposing)
            //{
            //}
            mDisposed = true;
        }

        public virtual HttpContext HttpContext => mAccessor.HttpContext;

        public virtual bool IsAuthenticated => HttpContext.User.Identity.IsAuthenticated;

        public virtual string UserId => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "sub").Value : AppConstants.USER_ID;

        public virtual string UserName => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "name").Value : AppConstants.NAME;

        public virtual string NickName => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "nick_name")?.Value : AppConstants.NICK_NAME;

        public virtual string MobilePhone => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "phone_number")?.Value : string.Empty;

        public virtual string Email => IsAuthenticated ? HttpContext.User.Claims.First(p => p.Type == "email")?.Value : string.Empty;
    }
}