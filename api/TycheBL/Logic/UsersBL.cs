/**
 * GNU General Public License Version 3.0, 29 June 2007
 * UsersBL
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
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tyche.TycheDAL.DataAccess;
using Tyche.TycheDAL.Models;
using Tyche.TycheDAL.Constants;
using Tyche.TycheDAL.Database;
using Tyche.TycheBL.Constants;

namespace Tyche.TycheBL.Logic
{
    public class UsersBL : BaseBL<UsersDal>
    {
        public UsersBL(string connectionString = null) : base(connectionString)
        {
            this.Dal = new UsersDal(this.ConnectionString);
        }

        public async Task<BlOperationResult<User>> CreateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException(BlConstants.UserIsNull);

            var predicate = new Predicate<User>(u => u.Username == user.Username || u.Email == user.Email);
            if (this.Dal.Exists(predicate))
                return Helper.Result<User>(ResponseCode.UserExists, null);

            var entity = await this.Dal.CreateUser(user);
            if (entity == null)
                return Helper.Result<User>(ResponseCode.DbError, null);

            try
            {
                var notificationsDal = new NotificationsDal(this.ConnectionString, this.Dal.Db);

                var notification = new Notification
                {
                    Type = NotificationType.Welcome,
                    Info = Messages.Welcome,
                    Created = DateTime.Now
                };

                var notificationEntity = await notificationsDal.CreateNotification(notification);
                if (notificationEntity != null)
                    await notificationsDal.AssignNotification(notificationEntity.Id, entity.Id);

                await notificationsDal.SaveChanges();

                return Helper.Result(entity);
            }
            catch (Exception ex)
            {
                return Helper.Result<User>(ResponseCode.Success, ex);
            }
        }

        public async Task<User> GetUserById(int userId)
        {
            if (userId < Restrictions.Id)
                throw new ArgumentException(BlConstants.InvalidID);

            return await this.Dal.GetUserById(userId);
        }

        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(BlConstants.InvalidUsername);

            return this.Dal.GetUserByUsername(username);
        }

        public async Task<List<User>> GetUsersByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                throw new ArgumentNullException(BlConstants.InvalidUsername);

            return await this.GetUsersPublicInfo(this.Dal.GetUsersByUsername(username));
        }

        public async Task<List<User>> GetUsersByUserIds(params int[] userIds)
        {
            if (userIds == null)
                throw new ArgumentNullException(BlConstants.UserIdsAreNull);

            if (userIds.Length == 0)
                throw new ArgumentNullException(BlConstants.UserIdsAreEmpty);

            return await this.GetUsersPublicInfo(this.Dal.GetUsersByIds(userIds));
        }

        private async Task<List<User>> GetUsersPublicInfo(IQueryable<User> users)
        {
            var query = users.Select(u => new User
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Username = u.Username,
                ProfilePictureUrl = u.ProfilePictureUrl
            });

            return await query.ToListAsync();
        }
    }
}
