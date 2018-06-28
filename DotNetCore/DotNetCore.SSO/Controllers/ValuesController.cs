using System.Collections.Generic;
using DotNetCore.Core.Base.Services.Cache;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Core.Events;
using DotNetCore.Domain.User;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.SSO.Controllers
{
    
    public class ValuesController : BaseController
    {
        private readonly IUserService mUserService;
        private readonly IRedisService mRedisService;

        public ValuesController(IUserService userService, IRedisService redisService)
        {
            mUserService = userService;
            mRedisService = redisService;
        }

        [Route("values/index")]
        public JsonResult Index()
        {
            var lResult = mRedisService.Get<List<UserProfile>>("UserInfo");
            if (lResult == null)
            {
                var lUser = mUserService.Get();
                mRedisService.Set("UserInfo", lUser, 60);
                lResult = lUser;
            }
            return Json(lResult);
        }

        [Route("values/send")]
        public JsonResult Send()
        {
            var lProducer = new EventProducer();
            var lResult = mRedisService.Get<List<UserProfile>>("UserInfo");
            lProducer.Publisher(lResult);
            return Json("success");
        }

        [Route("values/receive")]
        public JsonResult Receive()
        {
            var lProducer = new EventsConsumer();
            return Json(lProducer.Subscribe<List<UserProfile>>());
        }
    }
}
