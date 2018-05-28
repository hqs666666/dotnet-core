using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.SSO.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : BaseController
    {
        private readonly IUserService mUserService;

        public ValuesController(IUserService userService)
        {
            mUserService = userService;
        }

        public JsonResult Index()
        {
            return Json("mUserService.Get()");
        }
    }
}
