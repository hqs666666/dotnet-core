

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
        string CredentialsClientId { get; }
        string CredentialsSecret { get; }
        string PasswordClientId { get; }
        string PasswordSecret { get; }
        string PasswordKey { get; }
        string ImageType { get; }
        string AuthUrl { get; }
        string RedisConnection { get; }
        string RabbitMqHostName { get; }
        string RabbitMqUserName { get; }
        string RabbitMqPwd { get; }
    }
}
