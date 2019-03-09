/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Helper
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
using TycheBL.Models;

namespace TycheBL
{
    /// <summary>
    /// Helper class for some operations.
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Constructs database response
        /// </summary>
        /// <param name="responseCode">Response code</param>
        /// <param name="content">Content</param>
        /// <param name="exception">Exception</param>
        /// <returns>constructed database response</returns>
        internal static DbResponse ConstructDbResponse(
            ResponseCode responseCode,
            object content = null,
            Exception exception = null)
        {
            return new DbResponse
            {
                ResponseCode = responseCode,
                Content = content,
                Exception = exception
            };
        }

        /// <summary>
        /// Constructs database response
        /// </summary>
        /// <param name="responseCode">response code</param>
        /// <returns>database response</returns>
        internal static DbResponse ConstructDbResponse(ResponseCode responseCode)
        {
            return ConstructDbResponse(responseCode, Messages.Message(responseCode));
        }

        /// <summary>
        /// Constructs database error response
        /// </summary>
        /// <param name="exception">exception</param>
        /// <returns>database reponse</returns>
        internal static DbResponse ConstructDbError(Exception exception = null)
        {
            return ConstructDbResponse(ResponseCode.DbError, Messages.DbError, exception);
        }

        /// <summary>
        /// Creates notification
        /// </summary>
        /// <param name="notificationType">notification type</param>
        /// <param name="info">information</param>
        /// <param name="userIds">user IDs</param>
        /// <param name="chatRoomId">chatroom ID</param>
        /// <returns>notification</returns>
        internal static Notification ConstructNotification(
            NotificationType notificationType,
            List<int> userIds = null,
            string info = null,
            int? chatRoomId = null)
        {
            return new Notification
            {
                Type = notificationType,
                Info = info,
                UserIds = userIds,
                ChatRoomId = null
            };
        }

        /// <summary>
        /// Constructs notification
        /// </summary>
        /// <param name="notificationType">notification type</param>
        /// <param name="userId">user ID</param>
        /// <param name="info">notification information</param>
        /// <returns>notification</returns>
        internal static Notification ConstructNotification(
            NotificationType notificationType,
            int userId,
            string info = null,
            int? chatroomId = null)
        {
            return new Notification
            {
                Type = notificationType,
                Info = info,
                ChatRoomId = chatroomId,
                UserIds = new List<int>
                {
                    userId
                }
            };
        }
    }
}