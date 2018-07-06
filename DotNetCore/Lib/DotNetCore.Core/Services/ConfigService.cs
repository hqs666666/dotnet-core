


/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    配置文件接口实现
 * 
 ****************************************************************************/

using System;
using DotNetCore.Core.Base.Services;
using DotNetCore.FrameWork.Utils;

namespace DotNetCore.Core.Services
{
    public class ConfigService : IConfigService
    {
        #region IdentityServer

        public string CredentialsClientId => AppSettings.Configuration["AppSettings:ClientId1"];

        public string CredentialsSecret => AppSettings.Configuration["AppSettings:Secret1"];

        public string PasswordClientId => AppSettings.Configuration["AppSettings:ClientId2"];

        public string PasswordSecret => AppSettings.Configuration["AppSettings:Secret2"];

        public string AuthUrl => AppSettings.Configuration["Host:SSO"];

        #endregion

        #region Encrypt

        public string PasswordKey => AppSettings.Configuration["Encryption:PasswordKey"];

        #endregion

        #region File

        public string ImageType => AppSettings.Configuration["FileType:Image"];

        #endregion

        #region Redis

        public string RedisConnection => AppSettings.Configuration["ConnectionStrings:RedisConnection"];

        #endregion

        #region Rabbit

        public string RabbitMqHostName => AppSettings.Configuration["RabbitMQ:HostName"];

        public string RabbitMqUserName => AppSettings.Configuration["RabbitMQ:UserName"];

        public string RabbitMqPwd => AppSettings.Configuration["RabbitMQ:Password"];

        #endregion

        #region Email

        public string Name => AppSettings.Configuration["Email:Name"];
        public string Address => AppSettings.Configuration["Email:Address"];
        public string SecurityCode => AppSettings.Configuration["Email:SecurityCode"];
        public string SmtpHost => AppSettings.Configuration["Email:SMTPHost"];
        public int SmtpPort => Convert.ToInt32(AppSettings.Configuration["Email:SMTPPort"]);

        #endregion

    }
}
