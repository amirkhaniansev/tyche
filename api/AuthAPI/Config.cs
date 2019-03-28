using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;
using AuthAPI.Constant;

namespace AuthAPI
{
    /// <summary>
    /// Class for configuration data.
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets api resources.
        /// </summary>
        /// <returns>api resources</returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(Constants.OfflineAccess),
                new ApiResource(Constants.DataAPI)
            };
        }

        /// <summary>
        /// Gets clients.
        /// </summary>
        /// <returns>clients</returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = Constants.ClientId,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret(App.Secret.Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        Constants.DataAPI
                    },
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 15552000,
                    AccessTokenLifetime = 1800,
                    AllowOfflineAccess = true
                }
            };
        }

        /// <summary>
        /// Gets identity resources.
        /// </summary>
        /// <returns>identity resources</returns>
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResource
                {
                    Name = Constants.Username,
                    UserClaims = new List<string>
                    {
                        Constants.Username
                    }
                }
            };
        }
    }
}