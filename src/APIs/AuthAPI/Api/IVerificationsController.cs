/**
 * GNU General Public License Version 3.0, 29 June 2007
 * IVerificationsController
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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tyche.TycheDAL.Models;

namespace Tyche.AuthAPI.Api
{
    /// <summary>
    /// Interface that Verifications Controller should provide.
    /// </summary>
    public interface IVerificationsController
    {
        /// <summary>
        /// Posts new verification
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>action result</returns>
        Task<IActionResult> Post([FromBody]Verification verification);

        /// <summary>
        /// Updates verification info verifying user if key is correct and not expired.
        /// </summary>
        /// <param name="verification">verification</param>
        /// <returns>action result</returns>
        Task<IActionResult> Put([FromBody]Verification verification);
    }
}