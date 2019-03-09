/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ChatRoom
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
using System.ComponentModel.DataAnnotations.Schema;

namespace TycheBL.Models
{
    /// <summary>
    /// Model for describing chat creation
    /// </summary>
    public class ChatRoom
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets info if chat is group chat
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// Gets or sets picture url
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Gets or sets creator ID
        /// </summary>
        public int? CreatorId { get; set; }

        /// <summary>
        /// Gets or sets date and time when chatroom is created
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or setes chatroom members
        /// </summary>
        [NotMapped]
        public List<User> Members { get; set; }
    }
}