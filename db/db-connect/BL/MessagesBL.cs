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
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AccessCore.Repository;
using DbConnect.Models;

namespace DbConnect.BL
{
    /// <summary>
    /// Messages business logic
    /// </summary>
    public class MessagesBL : BaseBL
    {
        /// <summary>
        /// Creates new instance of <see cref="MessagesBL"/>
        /// </summary>
        /// <param name="dm">Data manager.</param>
        public MessagesBL(DataManager dm) : base(dm, BlType.MessagesBL)
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
                var result = await this.dm.OperateAsync<Message, object>(
                    nameof(DbOperation.CreateMessage),
                    message);

                var response = (ResponseCode)result;
                if (response != ResponseCode.Success)
                    return this.ConstructDbResponse(response, Messages.Message(response));

                return this.ConstructDbResponse(ResponseCode.Success);
            }
            catch (Exception ex)
            {
                return this.ConstructDbResponse(
                    ResponseCode.DbError,
                    Messages.DbError,
                    ex);
            }
        }
    }
}