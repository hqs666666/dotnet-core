using System;
using System.Threading.Tasks;
using DotNetCore.Core.Base;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace DotNetCore.FrameWork.Middleware
{
    public class ResponseMiddleware
    {
        private readonly RequestDelegate mNext;
        public ResponseMiddleware(RequestDelegate next)
        {
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
                await context.Response.WriteAsync(JsonConvert.SerializeObject(lEx));
            }
        }
    }
}