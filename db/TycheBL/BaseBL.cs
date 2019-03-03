/**
 * GNU General Public License Version 3.0, 29 June 2007
 * BaseBL
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
using AccessCore.Repository;

namespace TycheBL
{
    /// <summary>
    /// Base class for Business Logic
    /// </summary>
    public class BaseBL
    {
        /// <summary>
        /// Data manager
        /// </summary>
        protected readonly DataManager dm;

        /// <summary>
        /// Business logic type
        /// </summary>
        protected readonly BlType blType;

        /// <summary>
        /// Creates new instance of <see cref="BaseBL"/>
        /// </summary>
        /// <param name="dm">Data manager</param>
        /// <param name="blType">Business Logic type.</param>
        public BaseBL(DataManager dm, BlType blType = BlType.BaseBL)
        {
            this.dm = dm;
            this.blType = blType;
        }

        /// <summary>
        /// Constructs database response
        /// </summary>
        /// <param name="responseCode">Response code</param>
        /// <param name="content">Content</param>
        /// <param name="exception">Exception</param>
        /// <returns>constructed database response</returns>
        protected DbResponse ConstructDbResponse(
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
    }
}