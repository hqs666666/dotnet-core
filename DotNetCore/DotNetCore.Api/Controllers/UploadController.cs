

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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace DotNetCore.Api.Controllers
{
    [Produces("application/json")]
    [Consumes("application/json", "multipart/form-data")]
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
        public async Task<IActionResult> UploadImg(IFormFile file)
        {
            try
            {
                if (file.Length > 5242880)
                    return Ok(CreateErrorResultMsg(ApiErrorCode.Exception, "图片不得大于5M"));

                var lFilename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                var lExtName = lFilename.Substring(lFilename.LastIndexOf('.')).Replace("\"", "");

                if (!mConfigService.ImageType.Contains(lExtName.ToLower()))
                    return Ok(CreateErrorResultMsg(ApiErrorCode.Exception, "请上传正确的图片格式"));

                var lShortfilename = $"{StringUtils.NewGuid()}{lExtName}";
                var lDate = DateTime.Now.ToString("yyyy-MM-dd");
                var lFilePath = $@"{mHostingEnvironment.WebRootPath + AppConstants.FILE_PICTURE_URL + lDate}\";

                if (!Directory.Exists(lFilePath))
                    Directory.CreateDirectory(lFilePath);

                var lFileFullName = lFilePath + lShortfilename;

                using (var lFs = System.IO.File.Create(lFileFullName))
                {
                    await file.CopyToAsync(lFs);
                    await lFs.FlushAsync();
                }

                return Ok(CreateResultMsg(ApiErrorCode.Success));
            }
            catch (Exception lEx)
            {
                return Ok(lEx);
            }
        }
    }
}