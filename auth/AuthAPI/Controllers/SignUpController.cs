/**
 * GNU General Public License Version 3.0, 29 June 2007
 * SignUpController
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
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DbConnectClient;
using DbConnectClient.Models;
using LoggerService;

namespace AuthAPI.Controllers
{
    /// <summary>
    /// Controller for signing up API
    /// </summary>
    [ApiController]
    [Route("api/sign-up")]
    [Produces("application/json")]
    public class SignUpController : ControllerBase
    {
        /// <summary>
        /// Handler for sending POST request to register a user
        /// </summary>
        /// <param name="user">user</param>
        /// <returns>action result</returns>
        public async Task<IActionResult> Post([FromBody]User user)
        {
            try
            {
                if (user == null)
                    return this.BadRequest();

                user.PasswordHash = App.PasswordHasher.HashPassword(user.PasswordHash);
                var request = new Request<User>
                {
                    Input = user,
                    Operation = Operation.CreateUser
                };

                var response = await App.DataClient.SendRequestAsync(request);

                if (response.ResponseCode == ResponseCode.InternalError)
                    return this.StatusCode((int)HttpStatusCode.InternalServerError);

                if (response.ResponseCode == ResponseCode.Success)
                {
                    App.Logger.Log(Constants.UserCreated);
                    return this.Ok(response);
                }

                return this.BadRequest(response);
            }
            catch (Exception exception)
            {
                App.Logger.Log(LogHelper.CreateLog(
                    DateTime.Now, LogType.Fatal, Constants.InternalError, exception));
                return this.StatusCode(500);
            }
        }
    }
}