

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
    }
}
