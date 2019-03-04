/**
 * GNU General Public License Version 3.0, 29 June 2007
 * UsersBlTest
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

using System.Threading.Tasks;
using TycheBL;
using TycheBL.Context;
using TycheBL.Logic;
using TycheBL.Models;

namespace BusinessLogicTest
{
    static class UsersBlTest
    {
        internal static TycheContext Context = new TycheContext("Data Source=(local);Initial Catalog=TycheDB;Integrated Security=True");
        internal static async Task TestCreateUser()
        {
            var user = new User
            {
                FirstName = "Alita",
                LastName = "Petersen",
                Username = "alita",
                PasswordHash = "password",
                Email = "alita@gmail.com",
                ProfilePictureUrl = "empty"
            };

            var userbl = new UsersBL(Context);
            var response = await userbl.CreateUser(user);
            Print.PrintDbResponse("CreateUser", response);
        }

        internal static async Task TestCreateVerificationForUser()
        {
            var verification = new Verification
            {
                Code = "verification_code",
                UserId = 100000,
                ValidOffset = 50
            };

            var userbl = new UsersBL(Context);
            var response = await userbl.CreateVerificationForUser(verification);
            Print.PrintDbResponse("CreateVerificationForUser", response);
        }

        internal static async Task VerifiyUser()
        {
            var verification = new Verification
            {
                Code = "verification_code",
                UserId = 100000
            };

            var userbl = new UsersBL(Context);
            var response = await userbl.VerifyUser(verification);
            Print.PrintDbResponse("VerifyUser", response);
        }

        internal static async Task TestGetUserById()
        {
            var userBl = new UsersBL(Context);
            var response = await userBl.GetUserById(100000);
            Print.PrintDbResponse("GetUserById", response);
        }

        internal static async Task TestGetUserByUsername()
        {
            var userBl = new UsersBL(Context);
            var response = await userBl.GetUsersByUsername("sev");
            Print.PrintDbResponse("GetUserByUsername", response);
        }
    }
}