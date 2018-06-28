using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Api.Controllers
{
    [Authorize]
    public class ValuesController : BaseController
    {
        private readonly IUserService mUserService;

        public ValuesController(IUserService userService)
        {
            mUserService = userService;
        }
        [HttpGet("api/values")]
        public IActionResult Get()
        {
            return new JsonResult(UserId);
        }

    }
}
