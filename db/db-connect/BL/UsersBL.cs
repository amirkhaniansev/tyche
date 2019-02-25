/**
 * GNU General Public License Version 3.0, 29 June 2007
 * UsersBL
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
using System.Threading.Tasks;
using AccessCore.Repository;
using DbConnect.Models;

namespace DbConnect.BL
{
    /// <summary>
    /// Users business logic
    /// </summary>
    public class UsersBL : BaseBL
    {
        /// <summary>
        /// Creates new instance of <see cref="UsersBL"/>
        /// </summary>
        /// <param name="dm">Data manager</param>
        public UsersBL(DataManager dm)
            : base(dm, BlType.UsersBL)
        {
        }

        /// <summary>
        /// Creates user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>created user if everything is ok, otherwise null.</returns>
        public async Task<DbResponse> CreateUser(User user)
        {
            try
            {
                var result = await this.dm.OperateAsync<User, object>(
                    nameof(DbOperation.CreateUser),
                    user);
               
                var numericValue = (int)result;
                if (numericValue == 1)
                    return this.ConstructDbResponse(ResponseCode.UserExists, Messages.UserExists);

                if (numericValue == 2)
                    return this.ConstructDbResponse(ResponseCode.DbError, Messages.DbError);

                user.Id = numericValue;
                return this.ConstructDbResponse(ResponseCode.Success, user);
            }
            catch(Exception ex)
            {
                return this.ConstructDbResponse(
                    ResponseCode.DbError,
                    Messages.DbError,
                    ex);
            }
        }
    }
}
