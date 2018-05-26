using System;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.Core.Base.Services
{
    public interface IWorkContext : IDisposable
    {
        HttpContext HttpContext { get; }
        bool IsAuthenticated { get; }
        string UserId { get; }
        string UserName { get; }
        string NickName { get; }
        string MobilePhone { get; }
        string Email { get; }
    }
}