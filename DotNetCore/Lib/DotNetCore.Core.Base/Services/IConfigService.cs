

/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    配置文件接口
 * 
 ****************************************************************************/


namespace DotNetCore.Core.Base.Services
{
    public interface IConfigService
    {
        #region IdentityServer

        string CredentialsClientId { get; }
        string CredentialsSecret { get; }
        string PasswordClientId { get; }
        string PasswordSecret { get; }
        string AuthUrl { get; }

        #endregion

        #region Rabbit

        string RabbitMqHostName { get; }
        string RabbitMqUserName { get; }
        string RabbitMqPwd { get; }

        #endregion

        #region Redis

        string RedisConnection { get; }

        #endregion

        #region Encrypt

        string PasswordKey { get; }

        #endregion

        #region File

        string ImageType { get; }

        #endregion

        #region Email
        string Name { get; }
        string Address { get; }
        string SecurityCode { get; }
        string SmtpHost { get; }
        int SmtpPort { get; }

        #endregion

    }
}
