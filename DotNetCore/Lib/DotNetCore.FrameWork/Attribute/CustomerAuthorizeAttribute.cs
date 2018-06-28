using System;
using Microsoft.AspNetCore.Authorization;

namespace DotNetCore.FrameWork.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class CustomerAuthorizeAttribute : AuthorizeAttribute
    {
        public string Permission { get; set; }

        public CustomerAuthorizeAttribute(string permission)
        {
            Permission = permission;
        }
    }
}
