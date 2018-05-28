using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Attribute;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    //[CustomerAuthorize(AppConstants.ROLE_REGISTER_USER)]
    public class ValuesController : BaseController
    {
        private readonly IUserService mUserService;

        public ValuesController(IUserService userService)
        {
            mUserService = userService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(mUserService.Get());
        }
    }
}
