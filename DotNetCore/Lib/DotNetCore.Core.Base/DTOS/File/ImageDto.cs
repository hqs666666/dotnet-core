using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;

namespace DotNetCore.Core.Base.DTOS.File
{
    public class ImageBaseDto
    {
        public IFormFile File { get; set; }

        public string FileName()
        {
            var lFilename = ContentDispositionHeaderValue.Parse(File.ContentDisposition).FileName;
            var lExtName = lFilename.Substring(lFilename.LastIndexOf('.')).Replace("\"", "");
            return Guid.NewGuid().ToString("N") + lExtName;
        }

        public long Size => File.Length;
    }

    public class HeadImgDto : ImageBaseDto
    {
        public bool IsBig => Size > 5 * 1024 * 1024;

        public string Path => "/Static/Picture/";
    }
}
