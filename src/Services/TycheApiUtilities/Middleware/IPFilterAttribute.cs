/**
 * GNU General Public License Version 3.0, 29 June 2007
 * IPFilterAttribute
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
using Microsoft.AspNetCore.Mvc.Filters;
using Tyche.LoggerService;

namespace Tyche.TycheApiUtilities.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IPFilterAttribute : ActionFilterAttribute
    {
        public bool IsPublic { get; set; }
        
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as TycheApiController;
            var code = HttpStatusCode.Forbidden;

            try
            {
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress;

                var log = LogHelper.CreateLog(LogType.Fail, Messages.UserIPIsBlocked, null);
                
            }
            catch (Exception ex)
            {
                context.Result = controller.ApiErrorResponse(ex.Message);
            }
            finally
            {
                await base.OnActionExecutionAsync(context, next);
            }
        }
    }
}