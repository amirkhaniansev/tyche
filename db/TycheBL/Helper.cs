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