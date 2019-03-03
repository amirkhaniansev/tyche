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
using System.Threading.Tasks;
using AccessCore.Repository;
using TycheBL.Models;

namespace TycheBL
{
    /// <summary>
    /// Users business logic
    /// </summary>
    public class UsersBL : BaseBL
    {
        /// <summary>
        /// Creates new instance of <see cref="UsersBL"/>
        /// </summary>
        /// <param name="dm">Data manager</param>
        public UsersBL(DataManager dm)
            : base(dm, BlType.UsersBL)
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
                var result = await this.dm.OperateAsync<User, object>(
                    nameof(DbOperation.CreateUser),
                    user);

                var numeric = (int)result;

                if (numeric < 100000)
                {
                    var responseCode = (ResponseCode)numeric;
                    return Helper.ConstructDbResponse(
                        responseCode,
                        Messages.Message(responseCode));
                }

                user.Id = numeric;

                var notification = new Notification
                {
                    Type = NotificationType.Welcome,
                    Info = Messages.Welcome,
                    UserIds = new List<int>
                    {
                        user.Id
                    },
                };

                await this.CreateNotification(notification);

                return Helper.ConstructDbResponse(ResponseCode.Success, user);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(
                    ResponseCode.DbError,
                    Messages.DbError,
                    ex);
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
                verification.ValidOffset = 30;
                var result = await this.dm.OperateAsync<Verification, object>(
                   nameof(DbOperation.CreateVerificationCode),
                   verification);

                if (result == null)
                    return Helper.ConstructDbResponse(ResponseCode.VerificationCreationError);

                return Helper.ConstructDbResponse(ResponseCode.Success);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError, ex);
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
                var result = await this.dm.OperateAsync<Verification, object>(
                    nameof(DbOperation.VerifyUser),
                    verification);

                var numeric = (ResponseCode)result;

                if (numeric != ResponseCode.Success)
                    return Helper.ConstructDbResponse(numeric, Messages.Message(numeric));

                await this.CreateNotification(Helper.ConstructNotification(
                    NotificationType.VerificationSuccess,
                    verification.UserId,
                    Messages.VerificationSuccess));

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
                var result = await this.dm.OperateAsync<int, User>(
                    nameof(DbOperation.GetUserById),
                    id);

                var user = result as User;
                if (user == null)
                    return Helper.ConstructDbResponse(ResponseCode.UserNotExist, Messages.UserNotExists);

                user.PasswordHash = null;
                return Helper.ConstructDbResponse(ResponseCode.Success, user);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(
                    ResponseCode.DbError,
                    Messages.DbError,
                    ex);
            }
        }

        /// <summary>
        /// Gets user by username asynchronously.
        /// </summary>
        /// <param name="username">username</param>
        /// <returns>user</returns>
        public async Task<DbResponse> GetUserByUsername(string username)
        {
            try
            {
                var result = await this.dm.OperateAsync<string, User>(
                    nameof(DbOperation.GetUserByUsername),
                    username);

                var user = result as User;
                if (user == null)
                    return Helper.ConstructDbResponse(ResponseCode.UserNotExist, Messages.UserNotExists);

                user.PasswordHash = null;
                return Helper.ConstructDbResponse(ResponseCode.Success, user);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(
                    ResponseCode.DbError,
                    Messages.DbError,
                    ex);
            }
        } 
    }
}