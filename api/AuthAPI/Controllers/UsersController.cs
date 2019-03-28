/**
 * GNU General Public License Version 3.0, 29 June 2007
 * UsersController
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


using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TycheDAL.Models;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Controller for signing up API
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        /// <summary>
        /// Handler for sending POST request to register a user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>action result</returns>
        public async Task<IActionResult> Post([FromBody]User user)
        {
            throw new NotImplementedException();
        }
    }
}