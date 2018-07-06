using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.Core.Base.DTOS.User;
using DotNetCore.Core.Base.Services.Cache;
using DotNetCore.Core.Base.Services.Log;
using DotNetCore.Core.Base.Services.MessageQueue;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Domain.User;
using DotNetCore.FrameWork.Controller;
using DotNetCore.FrameWork.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.SSO.Controllers
{
    [Route("api/values")]
    public class ValuesController : BaseController
    {
        private readonly IUserService mUserService;
        private readonly IRedisService mRedisService;
        private readonly ILogService mLogService;
        private readonly IEventPublish mEventPublish;
        private readonly IEventSubscribe mEventSubscribe;

        public ValuesController(IUserService userService, IRedisService redisService,
            ILogService logService, IEventPublish eventPublish, IEventSubscribe eventSubscribe)
        {
            mUserService = userService;
            mRedisService = redisService;
            mLogService = logService;
            mEventPublish = eventPublish;
            mEventSubscribe = eventSubscribe;
        }

        [HttpGet("index")]
        public async Task<JsonResult> Index()
        {
            var lResult = mRedisService.Get<List<UserProfile>>("UserInfo");
            if (lResult == null)
            {
                var lUser = mUserService.GetList();
                await mRedisService.SetAsync("UserInfo", lUser, 60);
                lResult = lUser;
            }
            return Json(lResult);
        }

        [HttpGet("send")]
        public JsonResult Send()
        {
            var lUser = new UserDto
            {
                Id = StringUtils.NewGuid(),
                NickName = "刘城",
                Email = "891795565@qq.com"
            };
            mEventPublish.SendEmail(lUser);
            return Json("success");
        }

        [HttpGet("receive")]
        public JsonResult Receive()
        {
            mEventSubscribe.SendEmail();
            return Json("ok");
        }
    }
}
