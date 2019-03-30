/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Constants
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

namespace Tyche.AuthAPI.Constant
{
    /// <summary>
    /// Static class for constants
    /// </summary>
    public static class Constants
    {
        public const string DbConnectHost               = "DbConnectHost";
        public const string DbConnectPort               = "DbConnectPort";
        public const string EmailUsername               = "EmailUsername";
        public const string EmailPassword               = "EmailPassowrd";
        public const string AuthAPI                     = "AuthAPI";
        public const string LogPath                     = "LogPath";
        public const string UserCreated                 = "UserCreated";
        public const string InternalError               = "InternalError";
        public const string VerificationCodeCreated     = "Verification code is created";
        public const string ConnectionString            = "ConnectionString";
        public const string OfflineAccess               = "offline_access";
        public const string DataAPI                     = "DataAPI";
        public const string ClientId                    = "DefaultClient";
        public const string Username                    = "Username";
        public const string InvalidPassword             = "Invalid Password";
        public const string Custom                      = "custom";
        public const string UserNotExist                = "User does not exist.";
        public const string InvalidUsernameOrPassword   = "Invalid username or password";
        public const string UserNotVerified             = "User is not verified.";
        public const string AuthenticationSuccess       = "User is successfully authenticated";
        public const string UsersController             = "UsersController";
    }
}
