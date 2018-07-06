using DotNetCore.Core.Base;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Core.Base.Services;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Domain.User;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DotNetCore.Core.Base.Services.MessageQueue;
using DotNetCore.FrameWork.Helpers;
using DotNetCore.FrameWork.Utils;

namespace DotNetCore.Core.Services.User
{
    public class UserService : CoreService, IUserService
    {
        private readonly DbSet<Domain.User.User> mUserDbSet;
        private readonly DbSet<UserRole> mUserRoleDbSet;
        private readonly DbSet<Role> mRoleDbSet;
        private readonly DbSet<UserProfile> mUserProfileDbSet;
        private readonly IConfigService mConfigService;
        private readonly IPicResourceService mPicResourceService;
        private readonly IEventPublish mEventPublish;
        private readonly IMapper mMapper;
        public UserService(IWorkContext workContext, IDataContext dataContext,
            IConfigService configService, IPicResourceService picResourceService,
            IEventPublish eventPublish,IMapper mapper)
            : base(workContext, dataContext)
        {
            mUserDbSet = DataContext.Set<Domain.User.User>();
            mUserRoleDbSet = DataContext.Set<UserRole>();
            mRoleDbSet = DataContext.Set<Role>();
            mUserProfileDbSet = DataContext.Set<UserProfile>();
            mConfigService = configService;
            mPicResourceService = picResourceService;
            mEventPublish = eventPublish;
            mMapper = mapper;
        }

        public List<UserProfile> GetList()
        {
            return mUserProfileDbSet.ToList();
        }

        public async Task<UserProfile> GetAsync(string userId)
        {
            return await mUserProfileDbSet.FindAsync(userId);
        }

        public async Task<ResultMsg> ValidUser(string userName, string password)
        {
            var lPasswordHash = EncryptionHelper.Md5Encrypt($"{password}&{mConfigService.PasswordKey}");
            var lUser = await mUserDbSet.FirstOrDefaultAsync(p => p.UserName == userName && p.PasswordHash == lPasswordHash);
            if (lUser == null)
                return CreateErrorMsg("用户名与密码不匹配");

            var lResult = CreateResultMsg();
            lResult.Data = mMapper.Map<UserDto>(lUser);
            return lResult;
        }

        public IList<string> GetRole(string userId)
        {
            var lRoleIds = mUserRoleDbSet.Where(p => p.UserId == userId).Select(p => p.RoleId);
            return mRoleDbSet.Where(p => lRoleIds.Contains(p.Id)).Select(p => p.Name).ToList();
        }

        public ResultMsg UpdateHead()
        {
            return null;
        }

        public async Task<ResultMsg> Register(RegisterDto dto)
        {
            if (!dto.UserName.IsValidEmail())
                return CreateErrorMsg("邮箱格式不正确");

            var lExist = mUserDbSet.Any(p => p.UserName == dto.UserName);
            if (lExist)
                return CreateErrorMsg("用户已被注册");

            var lUser = Create<Domain.User.User>();
            lUser.PhoneNumber = "-1";
            lUser.PasswordHash = EncryptionHelper.Md5Encrypt($"{dto.Password}&{mConfigService.PasswordKey}");
            lUser.Email = dto.UserName;
            lUser.UserType = (int)UserType.Common;
            lUser.UserName = dto.UserName;
            lUser.NickName = dto.NickName;
            await mUserDbSet.AddAsync(lUser);

            var lUserProfile = Create<UserProfile>();
            lUserProfile.Id = lUser.Id;
            lUserProfile.PhoneNumber = "-1";
            lUserProfile.Email = dto.UserName;
            lUserProfile.UserName = dto.UserName;
            lUserProfile.NickName = dto.NickName;
            lUserProfile.Gender = (int)Gender.Unknown;
            lUserProfile.UserType = (int)UserType.Common;
            await mUserProfileDbSet.AddAsync(lUserProfile);

            var lRoleId = mRoleDbSet.FirstOrDefault(p => p.Name == AppConstants.ROLE_REGISTER_USER)?.Id;
            if (string.IsNullOrEmpty(lRoleId))
            {
                var lRole = Create<Role>();
                lRole.Name = AppConstants.ROLE_REGISTER_USER;
                lRole.DisplayName = "注册用户";
                await mRoleDbSet.AddAsync(lRole);
                lRoleId = lRole.Id;
            }

            var lUserRole = Create<UserRole>();
            lUserRole.UserId = lUser.Id;
            lUserRole.RoleId = lRoleId;
            await mUserRoleDbSet.AddAsync(lUserRole);

            var lResult = await SaveChangesAsync();
            if (!lResult.Result)
                return lResult;

            //将user信息发送给rabbit，rabbit接收到传来的消息后，发送邮件到相应的邮箱
            mEventPublish.SendEmail(mMapper.Map<UserDto>(lUser));
            return lResult;
        }
    }
}
