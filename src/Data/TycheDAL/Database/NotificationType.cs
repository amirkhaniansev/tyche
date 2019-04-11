/**
 * GNU General Public License Version 3.0, 29 June 2007
 * NotificationType
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

namespace Tyche.TycheDAL.Database
{
    /// <summary>
    /// Enum for notification type
    /// </summary>
    public enum NotificationType
    {
        Welcome             = 0x0,
        NewMessage          = 0x1,
        NewUserToChatroom   = 0x2,
        NewChatInvitation   = 0x3,
        VerificationSuccess = 0x4
    }
}
