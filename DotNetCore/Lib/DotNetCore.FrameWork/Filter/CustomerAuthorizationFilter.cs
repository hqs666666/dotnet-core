using System.Collections.Generic;
using System.Linq;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Attribute;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCore.FrameWork.Filter
{
    public class CustomerAuthorizationFilter : IAuthorizationFilter
    {
        private readonly IUserService mUserService;

        public CustomerAuthorizationFilter(IUserService userService)
        {
            mUserService = userService;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
            {
                return;
            }

            if (!(context.ActionDescriptor is ControllerActionDescriptor))
            {
                return;
            }
            var lAttributeList = new List<object>();
            lAttributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(true));
            lAttributeList.AddRange((context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.DeclaringType.GetCustomAttributes(true));
            var lAuthorizeAttributes = lAttributeList.OfType<CustomerAuthorizeAttribute>().ToList();
            if (!lAuthorizeAttributes.Any())
                return;

            var lClaims = context.HttpContext.User.Claims;

            //从claims取出用户相关信息，到数据库中取得用户具备的权限码，与当前Controller或Action标识的权限码做比较
            var lUserId = lClaims.FirstOrDefault(p => p.Type == "sub")?.Value;
            var lUserPermissions = mUserService.GetRole(lUserId);

            if (!lAuthorizeAttributes.Any(s => lUserPermissions.Contains(s.Permission)))
            {
                context.Result = new JsonResult("没有权限");
            }
        }
    }
}
