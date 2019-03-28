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
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TycheApiUtilities;
using TycheDAL.Models;
using TycheBL.Logic;
using TycheBL.Constants;
using LoggerService;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Controller for signing up API
    /// </summary>
    [ApiController]
    [Route("api/users")]
    [Produces("application/json")]
    public class UsersController : TycheApiController
    {
        /// <summary>
        /// Creates new instance of <see cref="UsersController"/>
        /// </summary>
        public UsersController() : base(App.Logger)
        {
        }

        /// <summary>
        /// Handler for sending POST request to register a user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>action result</returns>
        public async Task<IActionResult> Post([FromBody]User user)
        {
            try
            {
                using (var usersBl = new UsersBL(App.ConnectionString))
                {
                    user.PasswordHash = App.PasswordHasher.HashPassword(user.PasswordHash);

                    var result = await usersBl.CreateUser(user);

                    var response = new Response();
                    var logInfo = new LogInfo
                    {
                        Time = DateTime.Now
                    };

                    if (result.ResponseCode != ResponseCode.Success)
                    {
                        response.ResponseCode = (int)result.ResponseCode;
                        response.Content = Messages.Message(result.ResponseCode);

                        logInfo.LogType = LogType.Fail;
                        logInfo.Message = result.Exception?.Message;

                        return this.ApiResponse(HttpStatusCode.BadRequest, response, logInfo);
                    }

                    var u = result.Content;
                    response.ResponseCode = 0;
                    response.Content = new
                    {
                        u.Id,
                        u.FirstName,
                        u.LastName,
                        u.Username 
                    };

                    return this.ApiResponse(HttpStatusCode.OK, response);
                }
            }
            catch (Exception ex)
            {
                return this.ApiErrorResponse(ex.Message);
            }
        }
    }
}