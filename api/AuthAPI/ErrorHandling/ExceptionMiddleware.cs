/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ExceptionMiddleware
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
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Tyche.AuthAPI.Constant;
using Tyche.TycheApiUtilities;
using Tyche.TycheBL.Constants;

namespace Tyche.AuthAPI.ErrorHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate requestDelegate;

        public ExceptionMiddleware(RequestDelegate requestDelegate)
        {
            this.requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await this.requestDelegate(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = Production.Json;
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                App.Logger.Log(ex.Message);

                var response = new Response
                {
                    ResponseCode = ResponseCode.UnknownError,
                    Content = Messages.UnknownError
                };

                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }
    }
}
