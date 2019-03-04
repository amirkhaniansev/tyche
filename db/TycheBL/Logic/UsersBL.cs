/**
 * GNU General Public License Version 3.0, 29 June 2007
 * UsersBL
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *               <DavidPetr>       <david.petrosyan11100@gmail.com>
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TycheBL.Context;
using TycheBL.Models;

namespace TycheBL.Logic
{
    /// <summary>
    /// Users business logic
    /// </summary>
    public class UsersBL : BaseBL
    {
        /// <summary>
        /// Creates new instance of <see cref="UsersBL"/>
        /// </summary>
        /// <param name="context">context</param>
        public UsersBL(TycheContext context) : base(context, BlType.UsersBL)
        {
        }

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>created user if everything is ok, otherwise null.</returns>
        public async Task<DbResponse> CreateUser(User user)
        {
            try
            {
                if (this.Db.Users.Any(u => u.Username == user.Username && u.Email == user.Email))
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.UserExists,
                        Messages.UserExists);
                }

                var entry = await this.Db.Users.AddAsync(user);

                if (!await this.SaveChanges())
                {
                    return Helper.ConstructDbResponse(ResponseCode.DbError);
                }

                var notification = Helper.ConstructNotification(
                    NotificationType.Welcome, entry.Entity.Id, Messages.Welcome);

                var response = await this.CreateNotification(notification);
                response.Content = entry.Entity;
                return response;
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError, ex);
            }
        }

        /// <summary>
        /// Creates verification code for user asynchronously.
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> CreateVerificationForUser(Verification verification)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Verifies user asynchronously.
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> VerifyUser(Verification verification)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets user by ID asyncronously.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>user</returns>
        public async Task<DbResponse> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets users by username asynchronously.
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user</returns>
        public async Task<DbResponse> GetUsersByUsername(string username)
        {
            throw new NotImplementedException();
        } 
    }
}