

/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    登录注册相关控制器
 * 
 ****************************************************************************/

using System;
using System.Threading.Tasks;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Domain.User;
using DotNetCore.FrameWork.Attribute;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.SSO.Controllers
{
    [AllowAnonymous]
    [SecurityHeaders]
    [Route("api/account")]
    public class AccountController : BaseController
    {
        #region DI

        private readonly IUserService mUserService;

        #region Ctor

        public AccountController(IUserService userService)
        {
            mUserService = userService;
        }

        #endregion

        #endregion

        [Produces("application/json")]
        [HttpGet("login")]
        public async Task<IActionResult> LoginAsync([FromBody]LoginDto login)
        {
            if (IsAuthenticated)
                return Ok(CreateResultMsg(UserId, null));

            var lResult = await mUserService.ValidUser(login.UserName, login.Password);
            if (lResult.Result)
            {
                var lUser = (UserDto)lResult.Data;
                //写入cookie
                AuthenticationProperties lProps = null;    
                if (login.RememberMe)
                {
                    lProps = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(30))
                    };
                }
                await HttpContext.SignInAsync(lUser.Id, lUser.UserName, lProps);
                return Ok(CreateResultMsg(lUser.Id, null));
            }
            return Ok(CreateErrorResultMsg(ApiErrorCode.Exception));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterDto register)
        {
            if (!register.IsVaild)
                return Ok(CreateErrorResultMsg(ApiErrorCode.Exception, "error"));
            var lResult = await mUserService.Register(register);
            if (lResult.Result)
                return Ok(CreateResultMsg(lResult.Message));
            return Ok(CreateErrorResultMsg(ApiErrorCode.Exception, lResult.Message));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (IsAuthenticated)
            {
                await HttpContext.SignOutAsync();
            }
           
            return Ok(CreateResultMsg(IsAuthenticated));
        }
    }
}