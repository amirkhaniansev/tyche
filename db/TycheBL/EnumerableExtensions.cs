/**
 * GNU General Public License Version 3.0, 29 June 2007
 * EnumerableExtensions
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
using System.Collections.Generic;

namespace TycheBL
{
    /// <summary>
    /// Extensions for enumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Enumerates in collection invoking the given action for every element.
        /// </summary>
        /// <typeparam name="T">Type of entity</typeparam>
        /// <param name="enumerable">enumerable</param>
        /// <param name="action">action</param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            var enumerator = enumerable.GetEnumerator();
            if (enumerator == null)
                return;

            while (enumerator.MoveNext())
            {
                action.Invoke(enumerator.Current);
            }
        }
    }
}