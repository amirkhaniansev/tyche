/**
 * GNU General Public License Version 3.0, 29 June 2007
 * PasswordHasher
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

namespace Tyche.PasswordHasherService
{
    /// <summary>
    /// Service for password hashing
    /// </summary>
    public class PasswordHasher : IDisposable
    {
        /// <summary>
        /// Random number generation crypto service provide
        /// </summary>
        private readonly RNGCryptoServiceProvider _rng;

        /// <summary>
        /// Creates new instance of <see cref="PasswordHasher"/>
        /// </summary>
        public PasswordHasher()
        {
            this._rng = new RNGCryptoServiceProvider();
        }

        /// <summary>
        /// Does hash operation for password
        /// </summary>
        /// <param name="password">Password</param>
        /// <returns>hash of password</returns>
        public string HashPassword(string password)
        {
            // initializing salt
            var salt = new byte[16];

            // getting random bytes for salt
            this._rng.GetBytes(salt);

            // creating password-based key derivation function 2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                // getting bytes
                var hash = pbkdf2.GetBytes(20);
                
                // hashing
                var hashBytes = new byte[36];
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 20);

                // returning the hash of password
                return Convert.ToBase64String(hashBytes);
            }
        }

        /// <summary>
        /// Checks password
        /// </summary>
        /// <param name="password">Password</param>
        /// <param name="hashOfPassword">Hash of Password</param>
        /// <returns>boolean value indicating the validity of password.</returns>
        public bool CheckPassword(string password, string hashOfPassword)
        {
            // extracting the bytes
            var hashBytes = Convert.FromBase64String(hashOfPassword);

            // getting salt
            var salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            // computing the hash on the password user entered with  password-based key derivation function 2
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000))
            {
                var hash = pbkdf2.GetBytes(20);

                // comparing hashes
                for (int i = 0; i < 20; i++)
                {
                    // return false if there is no-matching hash
                    if (hashBytes[i + 16] != hash[i])
                        return false;
                }

                // otherwise return true
                return true;
            }
        }

        /// <summary>
        /// Disposes password hasher
        /// </summary>
        public void Dispose()
        {
            this._rng.Dispose();
        }
    }
}
