using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using DotNetCore.Core.Base.Services.User;
using DotNetCore.FrameWork.Attribute;
using DotNetCore.FrameWork.Controller;
using Microsoft.AspNetCore.Mvc;

namespace DotNetCore.Api.Controllers
{
    [Route("api/[controller]")]
    [CustomerAuthorize("Admin")]
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

        public string HttpPost(string charset = "UTF-8", string mediaType = "application/form-data")
        {
            string tokenUri = "/connect/token";
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:5000");

            var formData = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username", "17602132272"),
                new KeyValuePair<string, string>("password", "123456"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("client_id", "d7a8sd45sa4554da"),
                new KeyValuePair<string, string>("client_secret", "das4dasdw85511x5"),
                new KeyValuePair<string, string>("scope", "api")
            };

            HttpContent content = new FormUrlEncodedContent(formData);
            content.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
            content.Headers.ContentType.CharSet = charset;
            for (int i = 0; i < formData.Count; i++)
            {
                content.Headers.Add(formData[i].Key, formData[i].Value);
            }

            var res = client.PostAsync(tokenUri, content);
            res.Wait();
            HttpResponseMessage resp = res.Result;

            var res2 = resp.Content.ReadAsStringAsync();
            res2.Wait();

            string token = res2.Result;
            return token;
        }
    }
}
