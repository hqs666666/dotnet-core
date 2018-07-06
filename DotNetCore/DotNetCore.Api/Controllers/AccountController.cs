


/*****************************************************************************
 * 
 * Created On: 2018-06-01
 * Purpose:    
 * 
 ****************************************************************************/

using System.Threading.Tasks;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Controller;
using DotNetCore.FrameWork.Utils;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.Api.Controllers
{
    [Produces("application/json")]
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        private readonly IUserService mUserService;
        
        public AccountController(IUserService userService)
        {
            mUserService = userService;
        }

        [HttpPost("account/login")]
        public async Task<IActionResult> Login([FromBody]LoginDto login)
        {
            var lUser = await mUserService.ValidUser(login.UserName, login.Password);
            if (!lUser.Result)
                return Ok(CreateErrorResultMsg(ApiErrorCode.Exception, "用户名或密码不匹配"));
            var lToken = await GetToken(login);
            return Ok(CreateResultMsg(lToken));
        }

        private async Task<string> GetToken(LoginDto login)
        {
            var lUrl = await DiscoveryClient.GetAsync(ConfigService.AuthUrl);
            var lTokenClient = new TokenClient(lUrl.TokenEndpoint, ConfigService.PasswordClientId, ConfigService.PasswordSecret);
            var lTokenResponse = await lTokenClient.RequestResourceOwnerPasswordAsync(login.UserName,login.Password);
            if (lTokenResponse.IsError)
                return lTokenResponse.Error;
            return lTokenResponse.AccessToken;
        }

        private static IConfigService ConfigService => DI.ServiceProvider.GetRequiredService<IConfigService>();
    }
}