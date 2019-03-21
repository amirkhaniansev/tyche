/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Notification
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

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TycheDAL.Database;

namespace TycheDAL.Models
{
    /// <summary>
    /// Model for notification
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets notification type
        /// </summary>
        public NotificationType Type { get; set; }

        /// <summary>
        /// Gets or sets Info
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// Gets or sets Chatroom ID
        /// </summary>
        public int? ChatRoomId { get; set; }

        /// <summary>
        /// Gets or sest user ID
        /// </summary>
        [NotMapped]
        public List<int> UserIds { get; set; }
    }
}