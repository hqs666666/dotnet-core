

/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    返回用户信息
 * 
 ****************************************************************************/

using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.Core.Base.Services.Log;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Utils;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.SSO.Identity
{
    public class ProfileService : IProfileService
    {
        private readonly IUserService mUserService;

        public ProfileService(IUserService userService)
        {
            mUserService = userService;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                var lClaims = context.Subject.Claims.ToList();

                //set issued claims to return
                context.IssuedClaims = lClaims.ToList();

            }
            catch (Exception lEx)
            {
                await Task.Run(() =>
                {
                    LogService.Error(this, lEx);
                });
            }
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var lSub = context.Subject.GetSubjectId();
            var lUser = await mUserService.GetAsync(lSub);
            context.IsActive = lUser != null;
        }

        private ILogService LogService => DI.ServiceProvider.GetRequiredService<ILogService>();
    }
}
