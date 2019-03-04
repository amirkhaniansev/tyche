/**
 * GNU General Public License Version 3.0, 29 June 2007
 * BaseBL
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
using TycheBL.Models;
using TycheBL.Context;

namespace TycheBL.Logic
{
    /// <summary>
    /// Base class for Business Logic
    /// </summary>
    public class BaseBL
    {
        /// <summary>
        /// Tyche Context
        /// </summary>
        protected readonly TycheContext context;

        /// <summary>
        /// Business logic type
        /// </summary>
        protected readonly BlType blType;

        /// <summary>
        /// Gets Tyche context
        /// </summary>
        public TycheContext Db => this.context;

        /// <summary>
        /// Creates new instance of <see cref="BaseBL"/>
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="blType">Business Logic type.</param>
        public BaseBL(TycheContext context, BlType blType = BlType.BaseBL)
        {
            this.context = context;
            this.blType = blType;
        }

        /// <summary>
        /// Creates new notification
        /// </summary>
        /// <param name="notification">notification</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> CreateNotification(Notification notification)
        {
            try
            {
                var assignments = default(IQueryable<NotificationAssignment>);
                var entry = await this.Db.Notifications.AddAsync(notification);

                if (!await this.SaveChanges())
                {
                    return Helper.ConstructDbResponse(ResponseCode.DbError);
                }

                if (notification.ChatRoomId != null)
                {
                    var chatroom = await this.Db.ChatRooms.FindAsync(notification.ChatRoomId);
                    if (chatroom == null)
                    {
                        return Helper.ConstructDbResponse(
                            ResponseCode.ChatroomNotExist,
                            Messages.ChatroomNotExist);
                    }

                    assignments = this.Db.ChatroomMembers
                        .AsQueryable()
                        .Where(crm => crm.ChatRoomId == notification.ChatRoomId)
                        .Select(crm => new NotificationAssignment
                        {
                            NotificationId = entry.Entity.Id,
                            UserId = crm.UserId
                        });
                }
                else
                {
                    assignments = notification.UserIds?
                        .AsQueryable()
                        .Select(id => new NotificationAssignment
                        {
                            NotificationId = notification.Id,
                            UserId = id
                        });
                }

                await this.Db.NotificationAssignments.AddRangeAsync(assignments);

                if (!await this.SaveChanges())
                {
                    return Helper.ConstructDbResponse(ResponseCode.DbError);
                }

                return Helper.ConstructDbResponse(ResponseCode.Success);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(
                    ResponseCode.DbError, Messages.DbError, ex);
            }
        }

        /// <summary>
        /// Saves changes to database.
        /// </summary>
        /// <returns>database response</returns>
        public async Task<bool> SaveChanges()
        {
            using (var transaction = await this.context.Database.BeginTransactionAsync())
            {
                try
                {
                    await this.context.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }
    }
}