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
using System.Threading.Tasks;
using AccessCore.Repository;
using TycheBL.Models;

namespace TycheBL
{
    /// <summary>
    /// Base class for Business Logic
    /// </summary>
    public class BaseBL
    {
        /// <summary>
        /// Data manager
        /// </summary>
        protected readonly DataManager dm;

        /// <summary>
        /// Business logic type
        /// </summary>
        protected readonly BlType blType;

        /// <summary>
        /// Creates new instance of <see cref="BaseBL"/>
        /// </summary>
        /// <param name="dm">Data manager</param>
        /// <param name="blType">Business Logic type.</param>
        public BaseBL(DataManager dm, BlType blType = BlType.BaseBL)
        {
            this.dm = dm;
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
                var response = await this.dm.OperateAsync<Notification, object>(
                    nameof(DbOperation.CreateNotification), notification);

                var numeric = (int)response;
                
                if (numeric < 100000 && numeric == (int)ResponseCode.DbError)
                    return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError);

                var notificationId = numeric;
                var assignment = new NotificationAssignment
                {
                    NotificationId = notificationId
                };

                foreach (var userId in notification.UserIds)
                {
                    assignment.UserId = userId;
                    response = await this.dm.OperateAsync<NotificationAssignment, object>(
                        nameof(DbOperation.AssignNotificationToUser), assignment);

                    if ((ResponseCode)response == ResponseCode.DbError)
                        return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError);
                }

                return Helper.ConstructDbResponse(ResponseCode.Success, Messages.Success);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(ResponseCode.UnknownError, Messages.UnknownError, ex);
            }
        }        
    }
}