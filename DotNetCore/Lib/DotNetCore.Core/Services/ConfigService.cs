


/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    配置文件接口实现
 * 
 ****************************************************************************/

using DotNetCore.Core.Base.Services;
using DotNetCore.FrameWork.Utils;

namespace DotNetCore.Core.Services
{
    public class ConfigService : IConfigService
    {
        public string CredentialsClientId => AppSettings.Configuration["AppSettings:ClientId1"];

        public string CredentialsSecret => AppSettings.Configuration["AppSettings:Secret1"];

        public string PasswordClientId => AppSettings.Configuration["AppSettings:ClientId2"];

        public string PasswordSecret => AppSettings.Configuration["AppSettings:Secret2"];

        public string PasswordKey => AppSettings.Configuration["Encryption:PasswordKey"];

        public string ImageType => AppSettings.Configuration["FileType:Image"];

        public string AuthUrl => AppSettings.Configuration["Host:SSO"];
    }
}
