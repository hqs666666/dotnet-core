

/*****************************************************************************
 * 
 * Created On: 2018-05-30
 * Purpose:   文件上传 
 * 
 ****************************************************************************/

using System;
using System.IO;
using System.Threading.Tasks;
using DotNetCore.Core.Base;
using DotNetCore.Core.Base.Services;
using DotNetCore.FrameWork.Controller;
using DotNetCore.FrameWork.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DotNetCore.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]
    [AllowAnonymous]
    public class UploadController : BaseController
    {
        #region Cotr

        private readonly IHostingEnvironment mHostingEnvironment;
        private readonly IConfigService mConfigService;

        public UploadController(IHostingEnvironment hostingEnvironment,
            IConfigService configService)
        {
            mHostingEnvironment = hostingEnvironment;
            mConfigService = configService;
        }

        #endregion

        [HttpPost("upload/img")]
        public async Task<IActionResult> UploadImg(IFormFile files)
        {
            try
            {
                var lSize = files.Length;
                if (lSize > 5 * 1024 * 1024)
                    return Ok(CreateErrorResultMsg(ApiErrorCode.Exception, "图片不得大于5M"));

                var lFilename = ContentDispositionHeaderValue.Parse(files.ContentDisposition).FileName;
                var lExtName = lFilename.Substring(lFilename.LastIndexOf('.')).Replace("\"", "");

                if (!mConfigService.ImageType.Contains(lExtName.ToLower()))
                    return Ok(CreateErrorResultMsg(ApiErrorCode.Exception, "请上传正确的图片格式"));

                var lShortfilename = $"{StringUtils.NewGuid()}{lExtName}";
                var lDate = DateTime.Now.ToString("yyyy-MM-dd");
                var lFilePath = mHostingEnvironment.WebRootPath +
                                $@"{Directory.GetCurrentDirectory()}\wwwroot\Static\Pictures\{lDate}\";

                if (!Directory.Exists(lFilePath))
                    Directory.CreateDirectory(lFilePath);

                var lFileFullName = lFilePath + lShortfilename;

                using (var lFs = System.IO.File.Create(lFileFullName))
                {
                    await files.CopyToAsync(lFs);
                    await lFs.FlushAsync();
                }

                return Ok($"http://localhost:5001/wwwroot/Static/Pictures/{lDate}/{lShortfilename}");
            }
            catch (Exception lEx)
            {
                return Ok(lEx);
            }
        }
    }
}