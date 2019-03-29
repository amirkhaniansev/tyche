/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Config
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

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
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OfflineAccess,
                        Constants.DataAPI
                    },
                    UpdateAccessTokenClaimsOnRefresh = true,
                    RefreshTokenUsage = TokenUsage.ReUse,
                    AbsoluteRefreshTokenLifetime = 15552000,
                    AccessTokenLifetime = 1800,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false
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