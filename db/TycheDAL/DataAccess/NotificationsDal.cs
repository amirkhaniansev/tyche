/**
 * GNU General Public License Version 3.0, 29 June 2007
 * NotificationsDal
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

using System.Linq;
using System.Threading.Tasks;
using TycheDAL.Models;
using TycheDAL.Context;

namespace TycheDAL.DataAccess
{
    public class NotificationsDal : BaseDal
    {
        public NotificationsDal(string connectionString, TycheContext context = null) 
            : base(connectionString, context)
        {
        }

        public async Task<Notification> CreateNotification(Notification notification, bool saveAfterAdding = true)
        {
            return await this.AddEntity(notification, saveAfterAdding);
        }

        public async Task AssignNotification(long notificationId, params int[] userIds)
        {
            var assignments = userIds.Select(id => new NotificationAssignment
            {
                UserId = id,
                NotificationId = notificationId
            });

            await this.Db.NotificationAssignments.AddRangeAsync(assignments);
        }

        public IQueryable<Notification> GetUserNotifications(int userId)
        {
            var notifications = this.Db.Notifications.AsQueryable();
            var assignments = this.Db.NotificationAssignments.AsQueryable();

            var notSeen = assignments.Where(na => !na.IsSeen);

            var query = notSeen
                .Where(assignment => assignment.UserId == userId)
                .Join(
                    notifications,
                    assignment => assignment.NotificationId,
                    notification => notification.Id,
                    (assignment, notification) => notification);

            return query;
        }
    }
}