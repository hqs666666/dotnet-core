using System;
using System.Collections.Generic;
using System.Linq;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Domain.User;
using DotNetCore.FrameWork.Utils;
using Microsoft.EntityFrameworkCore;

namespace DotNetCore.Core.Services.User
{
    public class UserService : CoreService, IUserService
    {
        private readonly DbSet<Domain.User.User> mUserDbSet;
        private readonly DbSet<UserRole> mUserRoleDbSet;
        private readonly DbSet<Role> mRoleDbSet;
        private readonly DbSet<UserProfile> mUserProfileDbSet;
        private readonly IConfigService mConfigService;

        public UserService(IWorkContext workContext, IDataContext dataContext,
            IConfigService configService)
            : base(workContext, dataContext)
        {
            mUserDbSet = DataContext.Set<Domain.User.User>();
            mUserRoleDbSet = DataContext.Set<UserRole>();
            mRoleDbSet = DataContext.Set<Role>();
            mUserProfileDbSet = DataContext.Set<UserProfile>();
            mConfigService = configService;
        }

        public List<UserProfile> Get()
        {
            return mUserProfileDbSet.ToList();
        }

        public ResultMsg ValidUser(string userName, string password)
        {
            var lPasswordHash = EncryptionHelper.Md5Encrypt($"{password}&{mConfigService.PasswordKey}");
            var lUser = mUserDbSet.FirstOrDefault(p => p.PhoneNumber == userName && p.PasswordHash == lPasswordHash);
            if (lUser == null)
                return CreateErrorMsg("用户名与密码不匹配");

            var lResult = CreateResultMsg();
            lResult.Data = mUserProfileDbSet.Find(lUser.Id);
            return lResult;
        }

        public IList<string> GetRole(string userId)
        {
            var lRoleIds = mUserRoleDbSet.Where(p => p.UserId == userId).Select(p => p.RoleId);
            return mRoleDbSet.Where(p => lRoleIds.Contains(p.Id)).Select(p => p.DisplayName).ToList();
        }

        public ResultMsg Register(RegisterDto dto)
        {
            var lExist = mUserDbSet.Any(p => p.PhoneNumber == dto.UserName);
            if (lExist)
                return CreateErrorMsg("手机号已被注册");

            var lUser = Create<Domain.User.User>();
            lUser.PhoneNumber = dto.UserName;
            lUser.PasswordHash = EncryptionHelper.Md5Encrypt($"{dto.Password}&{mConfigService.PasswordKey}");
            lUser.Email = "-1";
            lUser.UserType = (int)UserType.Common;
            lUser.UserName = dto.UserName;
            lUser.NickName = dto.UserName;
            mUserDbSet.Add(lUser);

            var lUserProfile = Create<UserProfile>();
            lUserProfile.Id = lUser.Id;
            lUserProfile.PhoneNumber = dto.UserName;
            lUserProfile.Email = "-1";
            lUserProfile.UserName = dto.UserName;
            lUserProfile.NickName = dto.UserName;
            lUserProfile.Gender = (int)Gender.Unknown;
            lUserProfile.UserType = (int)UserType.Common;
            mUserProfileDbSet.Add(lUserProfile);

            var lRoleId = mRoleDbSet.FirstOrDefault(p => p.Name == AppConstants.ROLE_REGISTER_USER)?.Id;
            if (string.IsNullOrEmpty(lRoleId))
            {
                var lRole = Create<Role>();
                lRole.Name = AppConstants.ROLE_REGISTER_USER;
                lRole.DisplayName = "注册用户";
                mRoleDbSet.Add(lRole);
                lRoleId = lRole.Id;
            }

            var lUserRole = Create<UserRole>();
            lUserRole.UserId = lUser.Id;
            lUserRole.RoleId = lRoleId;
            mUserRoleDbSet.Add(lUserRole);

            return SaveChanges();
        }
    }
}
