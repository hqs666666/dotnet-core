using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services;
using DotNetCore.FrameWork.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.FrameWork.Controller
{
    public class BaseController : ControllerBase
    {
        private static IWorkContext WorkContext => DI.ServiceProvider.GetRequiredService<IWorkContext>();

        protected string UserId => WorkContext.UserId;

        protected bool IsAuthenticated => WorkContext.IsAuthenticated;

        protected ApiResultMsg<T> CreateResultMsg<T>(T data, ApiErrorCode errorCode, string message = null)
        {
            var lResultMsg = new ApiResultMsg<T>
            {
                ErrorCode = (int)errorCode,
                ErrorMsg = message ?? errorCode.ToString(),
                Data = data
            };
            return lResultMsg;
        }

        protected ApiResultMsg<T> CreateResultMsg<T>(T data, string message = null)
        {
            return CreateResultMsg<T>(data, ApiErrorCode.Success, message);
        }

        protected ApiResultMsg<string> CreateResultMsg(string message)
        {
            return CreateResultMsg<string>(null, ApiErrorCode.Success, message);
        }

        protected ApiResultMsg<string> CreateErrorResultMsg(ApiErrorCode errorCode, string message = null)
        {
            return CreateResultMsg<string>(null, errorCode, message);
        }
    }
}
