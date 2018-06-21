using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Attribute;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Api.Controllers
{
    [CustomerAuthorize(AppConstants.ROLE_REGISTER_USER)]
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
