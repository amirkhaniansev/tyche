/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Messages
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

using System.Collections.Generic;

namespace Tyche.TycheBL.Constants
{
    /// <summary>
    /// Class for storing constant messages
    /// </summary>
    public static class Messages
    {
        public const string NoSuchOperation             = "Operation is not supported.";
        public const string UnknownError                = "Unknown error occured.";
        public const string Success                     = "Operation is successfully done.";
        public const string UserExists                  = "User already exists.";
        public const string DbError                     = "Database error.";
        public const string VerificationCreationError   = "Unable to create verification code.";
        public const string VerificationCodeExpired     = "Verification Code Expired.";
        public const string UserAlreadyVerified         = "User is already verified.";
        public const string UserNotExists               = "User doesn't exist.";
        public const string NoContent                   = "No content";
        public const string Welcome                     = "Welcome to Tyche chat system.";
        public const string VerificationSuccess         = "You are successfullyy verified";
        public const string ChatroomNotExist            = "Chatroom doesn't exist.";
        public const string NewMessage                  = "New message";
        public const string ChatroomExists              = "Chatroom already exists.";
        public const string MemberIsAlreadyInChatroom   = "Member is already in chatroom.";
        public const string NewMemberIsAddedToChatroom  = "New member is added to chatroom.";
        public const string NotificationCreationError   = "Unable to create notification";
        public const string UnableToSendMail            = "Unable to send e-mail";

        private static Dictionary<ResponseCode, string> messages;

        static Messages()
        {
            messages = new Dictionary<ResponseCode, string>()
            {
                [ResponseCode.DbError]                      = DbError,
                [ResponseCode.NoSuchOperation]              = NoSuchOperation,
                [ResponseCode.Success]                      = Success,
                [ResponseCode.UnknownError]                 = UnknownError,
                [ResponseCode.UserAlreadyVerified]          = UserAlreadyVerified,
                [ResponseCode.UserExists]                   = UserExists,
                [ResponseCode.UserNotExist]                 = UserNotExists,
                [ResponseCode.VerificationCodeExpired]      = VerificationCodeExpired,
                [ResponseCode.VerificationCreationError]    = VerificationCreationError,
                [ResponseCode.NoContent]                    = NoContent,
                [ResponseCode.ChatroomNotExist]             = ChatroomNotExist,
                [ResponseCode.ChatroomExists]               = ChatroomExists,
                [ResponseCode.MemberIsAlreadyInChatroom]    = MemberIsAlreadyInChatroom,
                [ResponseCode.NotificationCreationError]    = NotificationCreationError,
                [ResponseCode.UnableToSendMail]             = UnableToSendMail
            };
        }

        /// <summary>
        /// Gets message with the given code
        /// </summary>
        /// <param name="code">response code</param>
        /// <returns>message</returns>
        public static string Message(ResponseCode code)
        {
            return messages.ContainsKey(code) ? messages[code] : string.Empty;
        }
    }
}
