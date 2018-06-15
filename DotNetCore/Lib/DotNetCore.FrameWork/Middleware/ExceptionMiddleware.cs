using System;
using System.IO;
using System.Threading.Tasks;
using DotNetCore.Core.Base.Services.Log;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.FrameWork.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate mNext;
        private readonly ILogService mLogService;
        public ExceptionMiddleware(RequestDelegate next, ILogService logService)
        {
            mLogService = logService;
            mNext = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await mNext.Invoke(context);
            }
            catch (Exception lEx)
            {
                var lReader = new StreamReader(context.Request.Body);
                var lRequestBody = await lReader.ReadToEndAsync();
                var lMessage = $"{context.Request.Host + context.Request.Path}；请求方式：{context.Request.Method}；请求参数：{lRequestBody}；";

                mLogService.Error(this, $"{lMessage}错误信息：", lEx);
            }
        }
    }
}