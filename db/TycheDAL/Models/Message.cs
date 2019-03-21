/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Message
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

namespace TycheDAL.Models
{
    /// <summary>
    /// Model for message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets user ID, who sent this message.
        /// </summary>
        public int From { get; set; }

        /// <summary>
        /// Gets or sets Chatroom ID, which this message is sent to.
        /// </summary>
        public int To { get; set; }

        /// <summary>
        /// Gets or sets header
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets or sets message text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets date and time of message creation.
        /// </summary>
        public DateTime Created { get; set; }
    }
}