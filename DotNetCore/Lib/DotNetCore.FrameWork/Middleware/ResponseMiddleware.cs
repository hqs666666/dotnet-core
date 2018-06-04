using System;
using System.IO;
using System.Threading.Tasks;
using DotNetCore.Core.Base.Services.Log;
using Microsoft.AspNetCore.Http;

namespace DotNetCore.FrameWork.Middleware
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate mNext;
        private readonly ILogService mLogService;
        public ResponseMiddleware(RequestDelegate next, ILogService logService)
        {
            mLogService = logService;
            mNext = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // var lStatusCode = context.Response.StatusCode;
            // if (lStatusCode == StatusCodes.Status500InternalServerError)
            // {
            //     var response = new ApiResultMsg<string>
            //     {
            //         ErrorCode = lStatusCode,
            //         ErrorMsg = "请求失败",
            //     };
            //     await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            // }
            //await mNext.Invoke(context);

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