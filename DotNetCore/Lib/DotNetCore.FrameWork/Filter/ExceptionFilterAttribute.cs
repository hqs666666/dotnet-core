
/*****************************************************************************
 * 
 * Created On: 2018-06-27
 * Purpose:    全局异常过滤器
 * 
 ****************************************************************************/

using System.IO;
using DotNetCore.Core.Base.Services.Log;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DotNetCore.FrameWork.Filter
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogService mLogService;

        public ExceptionFilter(ILogService logService)
        {
            mLogService = logService;
        }

        public override void OnException(ExceptionContext context)
        {
            var lReader = new StreamReader(context.HttpContext.Request.Body);
            var lRequestBody = lReader.ReadToEnd();
            var lMessage = $"{context.HttpContext.Request.Host + context.HttpContext.Request.Path}；请求方式：{context.HttpContext.Request.Method}；请求参数：{lRequestBody}；";

            mLogService.Error(this, $"{lMessage}错误信息：", context.Exception);

            context.Result = new JsonResult(context.Exception.Message);
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            base.OnException(context);
        }
    }
}
