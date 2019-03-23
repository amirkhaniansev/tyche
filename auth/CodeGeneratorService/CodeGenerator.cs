/**
 * GNU General Public License Version 3.0, 29 June 2007
 * CodeGenerator
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
using System.Security.Cryptography;

namespace CodeGeneratorService
{
    /// <summary>
    /// Code generator
    /// </summary>
    public class CodeGenerator : IDisposable
    {
        /// <summary>
        /// Random number generator service provider
        /// </summary>
        private readonly RNGCryptoServiceProvider rNGCryptoServiceProvider;

        /// <summary>
        /// Creates new instance of <see cref="CodeGenerator"/>
        /// </summary>
        public CodeGenerator()
        {
            this.rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Generates verification key.
        /// </summary>
        /// <param name="length">Length of key</param>
        /// <returns>Verification key</returns>
        public string GenerateVerifyKey(int length)
        {
            // buffer for storing random bytes
            var buffer = new byte[length];

            // getting random bytes
            this.rNGCryptoServiceProvider.GetBytes(buffer);

            // converting to string
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// Disposes code generator
        /// </summary>
        public void Dispose()
        {
            this.rNGCryptoServiceProvider.Dispose();
        }
    }
}