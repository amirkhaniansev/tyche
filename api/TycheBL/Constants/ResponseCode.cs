/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ResponseCode
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

namespace Tyche.TycheBL.Constants
{
    /// <summary>
    /// Enum for response codes
    /// </summary>
    public enum ResponseCode
    {
        Success                     = 0x0,
        UnknownError                = 0x1,
        NoSuchOperation             = 0x2,
        UserExists                  = 0x3,
        DbError                     = 0x4,
        VerificationCreationError   = 0x5,
        VerificationCodeExpired     = 0x6,
        UserNotExist                = 0x7,
        UserAlreadyVerified         = 0x8,
        NoContent                   = 0x9,
        ChatroomNotExist            = 0xA,
        ChatroomExists              = 0xB,
        MemberIsAlreadyInChatroom   = 0xC,
        NotificationCreationError   = 0xD,
        BlockedIPAddress            = 0xE,
        UnableToSendMail            = 0xF
    } 
}
