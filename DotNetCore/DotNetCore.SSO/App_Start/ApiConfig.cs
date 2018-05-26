

/*****************************************************************************
 * 
 * Created On: 2018-05-22
 * Purpose:    获取token相关配置
 * 
 ****************************************************************************/

using System.Collections.Generic;
using DotNetCore.FrameWork.Utils;
using IdentityServer4;
using IdentityServer4.Models;

namespace DotNetCore.SSO
{
    public class ApiConfig
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
                new IdentityResources.Profile()
            };
        }
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                //grant_type : client_credentials
                new Client
                {
                    ClientId = AppSettings.Configuration["AppSettings:ClientId1"],
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets =
                    {
                        new Secret(AppSettings.Configuration["AppSettings:Secret1"].Sha256())
                    },
                    AllowedScopes = { "api",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                    IdentityServerConstants.StandardScopes.Profile},

                },

                // resource owner password grant client
                new Client
                {
                    //grant_type : password

                    ClientId = AppSettings.Configuration["AppSettings:ClientId2"],
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret(AppSettings.Configuration["AppSettings:Secret2"].Sha256())
                    },
                    AllowedScopes = { "api",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                    IdentityServerConstants.StandardScopes.Profile }
                }
            };
        }
    }
}
