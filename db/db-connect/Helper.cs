/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Helper
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

namespace DbConnect
{
    /// <summary>
    /// Helper for server
    /// </summary>
    internal static class Helper
    {
        /// <summary>
        /// Constructs handler from BL function
        /// </summary>
        /// <typeparam name="TIn">Type of input.</typeparam>
        /// <param name="blFunction">BL function.</param>
        /// <returns>Handler</returns>
        internal static Func<object, Task<DbResponse>> CostructHandler<TIn>(Func<TIn, Task<DbResponse>> blFunction)
            where TIn : class
        {
            return async input => await blFunction(input as TIn);
        }
    }
}
