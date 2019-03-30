/**
 * GNU General Public License Version 3.0, 29 June 2007
 * TycheApiController
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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tyche.LoggerService;
using Tyche.TycheBL.Constants;

namespace Tyche.TycheApiUtilities
{
    public class TycheApiController : ControllerBase
    {
        private Logger logger;

        public TycheApiController(Logger logger)
        {
            this.logger = logger;
        }

        [NonAction]
        public ObjectResult ApiErrorResponse(string logMessage)
        {
            var log = new LogInfo
            {
                LogType = LogType.Fatal,
                Message = logMessage,
                Time = DateTime.Now
            };

            var response = new Response
            {
                Content = Messages.InternalError,
                ResponseCode = (int)ResponseCode.UnknownError
            };

            return this.ApiResponse(HttpStatusCode.InternalServerError, response, log);
        }

        [NonAction]
        public ObjectResult ApiResponse(HttpStatusCode httpStatusCode, Response response, LogInfo logInfo = null)
        {
            if (logInfo != null)
                this.logger.Log(logInfo);

            var json = JsonConvert.SerializeObject(response);
            return this.StatusCode((int)httpStatusCode, json);
        }
    }
}