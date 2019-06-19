/**
 * GNU General Public License Version 3.0, 29 June 2007
 * VerificationsController
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
using Tyche.TycheDAL.Models;
using Tyche.TycheBL.Logic;
using Tyche.TycheBL.Constants;
using Tyche.TycheApiUtilities;
using Tyche.AuthAPI.Api;
using Tyche.AuthAPI.Constant;

namespace Tyche.AuthAPI.Controllers
{
    /// <summary>
    /// Controller for verifications
    /// </summary>
    [ApiController]
    [Route(Routes.Verifications)]
    [Produces(Production.Json)]
    public class VerificationsController : TycheApiController, IVerificationsController
    {
        /// <summary>
        /// Creates new instance of <see cref="VerificationsController"/>
        /// </summary>
        /// <param name="logger">logger</param>
        public VerificationsController() : base(App.Logger)
        {
        }

        /// <summary>
        /// Posts new verification
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>action result</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Verification verification)
        {
            if (verification == null)
                return this.ApiErrorResponse(Constants.VerifiactionIsNull);
            
            using (var usersBl = new UsersBL(App.ConnectionString))
            {
                var response = new Response();
                var user = await usersBl.GetUserById(verification.UserId);
                if (user == null)
                    return this.ApiErrorResponse(HttpStatusCode.BadRequest, ResponseCode.UserNotExist);

                verification.Code = App.CodeGenerator.GenerateKey(24);
                var v = await usersBl.CreateVerificationForUser(verification);

                if (v == null)
                    return this.ApiErrorResponse(Messages.DbError);

                if (!await App.Mailer.Send(user.Email, v.Code))
                    return this.ApiErrorResponse(HttpStatusCode.Conflict, ResponseCode.UnableToSendMail);

                return this.ApiSuccessResponse();
            }
        }

        /// <summary>
        /// Updates verification info verifying user if key is correct and not expired.
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>action result</returns>
        public async Task<IActionResult> Put([FromBody]Verification verification)
        {
            if (verification == null)
                return this.ApiErrorResponse(Constants.VerifiactionIsNull);

            using (var usersBl = new UsersBL(App.ConnectionString))
            {
                var user = await usersBl.GetUserById(verification.UserId);
                var response = new Response();
                if (user == null)
                    return this.ApiErrorResponse(HttpStatusCode.BadRequest, ResponseCode.UserNotExist);

                if (user.IsVerified)
                    return this.ApiErrorResponse(HttpStatusCode.AlreadyReported, ResponseCode.UserAlreadyVerified);
               
                var v = usersBl.GetVerificationInfo(verification.UserId, verification.Code);

                if (v.Created.AddMinutes(v.ValidOffset) >= DateTime.Now)
                    this.ApiErrorResponse(HttpStatusCode.NotAcceptable, ResponseCode.VerificationCodeExpired);

                if (!await usersBl.VerifyUser(v, user))
                    return this.ApiErrorResponse(HttpStatusCode.Conflict, ResponseCode.DbError);
                
                return this.ApiSuccessResponse();             
            }
        }
    }
}