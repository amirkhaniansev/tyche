/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ResourceOwnerPasswordValidator
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

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Tyche.AuthAPI.Constant;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Tyche.TycheDAL.Models;

namespace Tyche.AuthAPI.Authentication
{
    /// <summary>
    /// Resource owner password validator
    /// </summary>
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        /// <summary>
        /// User repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructs new instance of 
        /// <see cref="ResourceOwnerPasswordValidator"/> with the given user repoistory.
        /// </summary>
        /// <param name="userRepository">User Repositor</param>
        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            this._userRepository = userRepository; 
        }

        /// <summary>
        /// Validates context
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Validation task.</returns>
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            try
            {
                // getting user
                var user = await this._userRepository.FindAsync(context.UserName);

                // checking password
                if (user == null)
                {
                    // message about non-existing user
                    context.Result = new GrantValidationResult(
                        TokenRequestErrors.InvalidGrant, Constants.UserNotExist);

                    App.Logger.LogError(Constants.UserNotExist);

                    return;
                }

                if (!user.IsVerified)
                {
                    context.Result = new GrantValidationResult(
                        TokenRequestErrors.InvalidClient, Constants.UserNotVerified);

                    App.Logger.LogError(Constants.UserNotVerified);

                    return;
                }

                // if password is ok set
                if (!App.PasswordHasher.CheckPassword(context.Password, user.PasswordHash))
                {
                    // othwerwise construct error response
                    context.Result = new GrantValidationResult(
                        TokenRequestErrors.InvalidGrant, Constants.InvalidPassword);

                    App.Logger.LogError(Constants.InvalidPassword);

                    return;
                }

                context.Result = new GrantValidationResult(
                     user.Id.ToString(), Constants.Custom, GetUserClaims(user));

                App.Logger.Log(Constants.AuthenticationSuccess);
            }
            catch (Exception ex)
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant, Constants.InvalidUsernameOrPassword);

                App.Logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Constructs claims with the given user.
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Claims</returns>
        public static Claim[] GetUserClaims(User user)
        {
            // constructing and returning claims
            return new Claim[]
            {
                new Claim("user_id", user.Id.ToString()),
                new Claim(JwtClaimTypes.Name, user.Username)
            };
        }
    }
}