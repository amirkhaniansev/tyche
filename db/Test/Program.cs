/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Program
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

using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;
using TycheBL;
using TycheBL.Logic;
using TycheBL.Models;

namespace Test
{
    class Program
    {
        static string cnn = "Data Source=(local);Initial Catalog=TycheDB;Integrated Security=True";

        static void Print(string name, DbResponse dbResponse)
        {
            Console.WriteLine(name + ":");
            Console.WriteLine(JsonConvert.SerializeObject(dbResponse, Formatting.Indented));
            Console.ReadLine();
        }
        
        static async Task TestCreateUser()
        {
            using (var bl = new UsersBL(cnn))
            {
                var user = new User
                {
                    Username = "pablo",
                    FirstName = "Pablo",
                    LastName = "Fernandez",
                    Email = "pablo@gmail.com",
                    ProfilePictureUrl = "",
                    PasswordHash = "password"
                };

                var response = await bl.CreateUser(user);

                Print("CreateUser", response);
            };
        }

        static async Task TestCreateVerification()
        {
            using (var bl = new UsersBL(cnn))
            {
                var verification = new Verification
                {
                    Created = DateTime.Now,
                    Code = "code",
                    UserId = 100007,
                    ValidOffset = 30
                };

                var response = await bl.CreateVerificationForUser(verification);

                Print("CreateVerification", response);
            }
        }

        static async Task TestVerifyUser()
        {
            using (var bl = new UsersBL(cnn))
            {
                var verification = new Verification
                {
                    Created = DateTime.Now,
                    Code = "code",
                    UserId = 100007
                };

                var response = await bl.VerifyUser(verification);
                
                Print("VerifyUser", response);
            }
        }

        static async Task TestGetUserById()
        {
            using (var bl = new UsersBL(cnn))
            {
                var response = await bl.GetUserById(100007);

                Print("GetUserById", response);
            }
        }

        static async Task TestGetUsersByUsername()
        {
            using (var bl = new UsersBL(cnn))
            {
                var response = await bl.GetUsersByUsername("pab");

                Print("GetUsersByUsername", response);
            }
        }

        static async Task TestCreateMessage()
        {
            using (var bl = new MessagesBL(cnn))
            {
                var message = new Message
                {
                    Text = "NewMessage",
                    Created = DateTime.Now,
                    From = 100007,
                    To = 2
                };

                var response = await bl.CreateMessage(message);

                Print("CreateMessage", response);
            }
        }

        static async Task TestGetMessages()
        {
            using (var bl = new MessagesBL(cnn))
            {
                var filter = new MessageFilter
                {
                    ChatroomId = 2,
                    FromDate = DateTime.MinValue,
                    ToDate = DateTime.Now
                };

                var response = await bl.GetMessages(filter);
                
                Print("GetMessages", response);
            }
        }

        static async Task TestCreateChatroom()
        {
            using (var bl = new ChatroomsBL(cnn))
            {
                var chatroom = new ChatRoom
                {
                    Created = DateTime.Now,
                    CreatorId = 100007,
                    IsGroup = true,
                    Name = "ChatRoom",
                    PictureUrl = ""
                };

                var response = await bl.CreateChatroom(chatroom);

                Print("CreateChatroom", response);
            }
        }

        static async Task TestGetUserChatrooms()
        {
            using (var bl = new ChatroomsBL(cnn))
            {
                var response = await bl.GetChatroomsByUserId(100007);

                Print("GetUserChatrooms", response);
            }
        }

        static async Task TestGetChatroom()
        {
            using (var bl = new ChatroomsBL(cnn))
            {
                var response = await bl.GetChatroomById(2);

                Print("GetChatroom", response);
            }
        }

        static async Task TestAddMemberToChatroom()
        {
            using (var bl = new ChatroomsBL(cnn))
            {
                var crm = new ChatRoomMember
                {
                    ChatRoomId = 2,
                    UserId = 100000
                };

                var response = await bl.AddMemberToChatroom(crm);

                Print("AddMemberToChatroom", response);
            }
        }

        static void Main(string[] args)
        {
            TestCreateUser().Wait();
            TestCreateVerification().Wait();
            TestVerifyUser().Wait();
            TestGetUserById().Wait();
            TestGetUsersByUsername().Wait();
            TestCreateMessage().Wait();
            TestGetMessages().Wait();
            TestCreateChatroom().Wait();
            TestGetUserChatrooms().Wait();
            TestGetChatroom().Wait();
            TestAddMemberToChatroom().Wait();
        }
    }
}
