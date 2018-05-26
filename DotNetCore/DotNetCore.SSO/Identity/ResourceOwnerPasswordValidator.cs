﻿

/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    验证设置用户Claim
 * Message：IdentityServer提供了接口访问用户信息，但是默认返回的数据只有sub，就是上面设置的subject: context.UserName，要返回更多的信息，需要实现IProfileService接口
 *
 ****************************************************************************/

using System.Security.Claims;
using System.Threading.Tasks;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Domain.User;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace DotNetCore.SSO.Identity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserService mUserService;

        public ResourceOwnerPasswordValidator(IUserService userService)
        {
            mUserService = userService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            //根据context.UserName和context.Password与数据库的数据做校验，判断是否合法
            var lResult = mUserService.ValidUser(context.UserName, context.Password);
            if (lResult.Result)
            {
                context.Result = new GrantValidationResult(
                    subject: context.UserName,
                    authenticationMethod: "custom",
                    claims: GetUserClaims((UserProfile)lResult.Data));
            }
            else
            {

                //验证失败dot
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");
            }
        }
        //可以根据需要设置相应的Claim
        private Claim[] GetUserClaims(UserProfile user)
        {
            return new Claim[]
            {
                new Claim(JwtClaimTypes.Subject, user.Id),
                new Claim(JwtClaimTypes.Name,user.UserName),
                new Claim(JwtClaimTypes.NickName, user.NickName),
                new Claim(JwtClaimTypes.Email, user.Email),
                new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber)
            };
        }
    }
}