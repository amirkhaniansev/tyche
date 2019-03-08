/**
 * GNU General Public License Version 3.0, 29 June 2007
 * MessagesBL
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
    /// Messages business logic
    /// </summary>
    public class MessagesBL : BaseBL
    {
        /// <summary>
        /// Creates new instance of <see cref="MessagesBL"/>
        /// </summary>
        /// <param name="connectionString">Connection String</param>
        public MessagesBL(string connectionString) : base(connectionString, BlType.MessagesBL)
        {
        }

        /// <summary>
        /// Creates message asynchronously.
        /// </summary>
        /// <param name="message">message</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> CreateMessage(Message message)
        {
            try
            {
                var chatroom = await this.Db.ChatRooms.FindAsync(message.To);
                if (chatroom == null)
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.ChatroomNotExist, 
                        Messages.ChatroomNotExist);
                }

                var msg = await this.Db.Messages.AddAsync(message);

                var notification = new Notification
                {
                    ChatRoomId = message.To,
                    Info = Messages.NewMessage,
                    Type = NotificationType.NewMessage
                };

                await this.CreateNotification(notification);

                return Helper.ConstructDbResponse(ResponseCode.Success);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError, ex);
            }
        }

        /// <summary>
        /// Gets messages by the given message filter.
        /// </summary>
        /// <param name="messageFilter">message filter.</param>
        /// <returns>messages</returns>
        public async Task<DbResponse> GetMessages(MessageFilter messageFilter)
        {
            try
            {
                if (!this.Db.ChatRooms.Any(cr => cr.Id == messageFilter.ChatroomId))
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.ChatroomNotExist,
                        Messages.ChatroomNotExist);
                }

                var messages = await this.Db
                    .Messages
                    .Where(m =>
                        m.To == messageFilter.ChatroomId &&
                        m.Created > messageFilter.FromDate &&
                        m.Created < messageFilter.ToDate)
                     .ToListAsync();

                if (messages == null || !messages.Any())
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.NoContent,
                        Messages.NoContent);
                }

                return Helper.ConstructDbResponse(ResponseCode.Success, messages);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbResponse(ResponseCode.DbError, Messages.DbError, ex);
            }
        }
    }
}