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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        /// <param name="connectionString">connection string</param>
        public UsersBL(string connectionString) : base(connectionString, BlType.UsersBL)
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
            try
            {
                var entry = await this.Db.Verifications.AddAsync(verification);

                if (!await this.SaveChanges())
                {
                    return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError);
                }

                return Helper.ConstructDbResponse(ResponseCode.Success, Messages.Success);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(
                    ResponseCode.DbError, 
                    Messages.VerificationCreationError, 
                    ex);
            }
        }

        /// <summary>
        /// Verifies user asynchronously.
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> VerifyUser(Verification verification)
        {
            try
            {
                var user = await this.Db.Users.FindAsync(verification.UserId);
                if (user == null)
                {
                    return Helper.ConstructDbResponse(ResponseCode.UserNotExist, 
                        Messages.UserNotExists);
                }

                if (user.IsVerified)
                {
                    return Helper.ConstructDbResponse(ResponseCode.UserAlreadyVerified, 
                        Messages.UserAlreadyVerified);
                }

                var verificationInfo = this.Db.Verifications.FirstOrDefault(
                   v => v.UserId == verification.UserId && v.Code == verification.Code);

                if (verificationInfo == null || 
                    verificationInfo.Created.AddMinutes(verification.ValidOffset) >= DateTime.Now)
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.VerificationCodeExpired,
                        Messages.VerificationCodeExpired);
                }

                user.IsVerified = true;

                this.Db.Verifications.Remove(verificationInfo);
                this.Db.Users.Update(user);

                if (!await this.SaveChanges())
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.DbError,
                        Messages.DbError);
                }

                return Helper.ConstructDbResponse(ResponseCode.Success);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError, ex);
            }
        }

        /// <summary>
        /// Gets user by ID asyncronously.
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>user</returns>
        public async Task<DbResponse> GetUserById(int id)
        {
            try
            {
                var user = await this.Db.Users.FindAsync(id);
                if (user == null)
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.UserNotExist,
                        Messages.UserNotExists);
                }

                user.PasswordHash = null;
                return Helper.ConstructDbResponse(ResponseCode.Success, user);
            }
            catch(Exception ex)
            {
                return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError, ex);
            }
        }

        /// <summary>
        /// Gets users by username asynchronously.
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user</returns>
        public async Task<DbResponse> GetUsersByUsername(string username)
        {
            try
            {
                var users = await this.Db.Users
                    .Where(u => u.Username.Contains(username))
                    .Select(user => new
                    {
                        user.Id,
                        user.IsVerified,
                        user.FirstName,
                        user.LastName,
                        user.ProfilePictureUrl,
                        user.Username
                    })
                    .ToListAsync();

                if (users == null || !users.Any())
                {
                    return Helper.ConstructDbResponse(ResponseCode.NoContent,
                        Messages.NoContent);
                }

                return Helper.ConstructDbResponse(ResponseCode.Success, users);
            }
            catch(Exception ex)
            {
                return Helper.ConstructDbResponse(
                    ResponseCode.DbError, 
                    Messages.DbError, 
                    ex);
            }
        } 
    }
}