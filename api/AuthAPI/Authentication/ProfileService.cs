/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ProfileService
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
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Tyche.TycheDAL.Constants;

namespace Tyche.AuthAPI.Authentication
{
    /// <summary>
    /// Profile serviice
    /// </summary>
    public class ProfileService:IProfileService
    {
        /// <summary>
        /// User repository
        /// </summary>
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Creates new instance of 
        /// <see cref="ProfileService"/> with the given user repository.
        /// </summary>
        /// <param name="userRepository">User repository</param>
        public ProfileService(IUserRepository userRepository)
        {
           this. _userRepository = userRepository;
        }

        /// <summary>
        /// Gets profile data.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>profile data getting task</returns>
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                // depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    // get user from db (in my case this is by email)
                    var user = await this. _userRepository.FindAsync(context.Subject.Identity.Name);

                    // checking user
                    if (user != null)
                    {
                        var claims = ResourceOwnerPasswordValidator.GetUserClaims(user);

                        // set issued claims to return
                        context.IssuedClaims.AddRange(claims);
                    }
                }
                else
                {
                    // get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    // where and subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId?.Value) && int.Parse(userId.Value) > 0)
                    {
                        // get user from db (find user by user id)
                        var user = await this._userRepository.FindAsync(int.Parse(userId.Value));

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = ResourceOwnerPasswordValidator.GetUserClaims(user);

                            context.IssuedClaims.AddRange(claims);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.Logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Checks if user is active.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>activity checking task</returns>
        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                // get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                if (string.IsNullOrEmpty(userId?.Value) || int.Parse(userId.Value) < Restrictions.Id)
                    return;

                var user = await this._userRepository.FindAsync(int.Parse(userId.Value));

                if (user == null || !user.IsActive)
                    return;

                context.IsActive = user.IsActive;
            }
            catch (Exception ex)
            {
                App.Logger.LogError(ex.Message);
            }
        }
    }
}
