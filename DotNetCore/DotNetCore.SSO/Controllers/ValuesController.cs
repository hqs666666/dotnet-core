using System.Collections.Generic;
using System.Threading.Tasks;
using DotNetCore.Core.Base.Services.Cache;
using DotNetCore.Core.Base.Services.Log;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.Core.Events;
using DotNetCore.Domain.User;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.SSO.Controllers
{
    [Route("api/values")]
    public class ValuesController : BaseController
    {
        private readonly IUserService mUserService;
        private readonly IRedisService mRedisService;
        private readonly ILogService mLogService;

        public ValuesController(IUserService userService, IRedisService redisService, ILogService logService)
        {
            mUserService = userService;
            mRedisService = redisService;
            mLogService = logService;
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
            var lProducer = new EventProducer();
            var lResult = mRedisService.GetAsync<List<UserProfile>>("UserInfo");
            lProducer.Publisher(lResult);
            return Json("success");
        }

        [HttpGet("receive")]
        public JsonResult Receive()
        {
            var lProducer = new EventsConsumer();
            return Json(lProducer.Subscribe<List<UserProfile>>());
        }
    }
}
