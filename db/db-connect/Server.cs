/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Server
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

using TycheBL;
using TycheBL.Logic;
using TycheBL.Models;
using DbConnect.Config;

namespace DbConnect
{
    /// <summary>
    /// Class for holding global information
    /// </summary>
    public static class Server
    {
        /// <summary>
        /// Gets or sets Tyche Config
        /// </summary>
        public static TycheConfig TycheConfig { get; set; }

        /// <summary>
        /// Gets or sets Data Server builder
        /// </summary>
        public static DataServerBuilder DataServerBuilder { get; set; }

        /// <summary>
        /// Gets or sets Server
        /// </summary>
        public static DataServer DataServer { get; set; }

        /// <summary>
        /// Initializes configs
        /// </summary>
        public static void InitConfigs()
        {
            TycheConfig = new TycheConfig("./Config/config.xml");
        }

        /// <summary>
        /// Builds data server
        /// </summary>
        public static void Build()
        {
            DataServer = DataServerBuilder.Build();
        }

        /// <summary>
        /// Starts server
        /// </summary>
        public static void Start()
        {
            DataServer.Run();
        }

        /// <summary>
        /// Prepares the server for starting.
        /// </summary>
        public static void InitBuilder()
        {
            DataServerBuilder = new DataServerBuilder()
                .AssignIp(TycheConfig.Host)
                .AssignPort(TycheConfig.Port);

            DataServerBuilder = DataServerBuilder
                .AddOperation<User>(DbOperation.CreateUser)
                .AddOperation<Verification>(DbOperation.CreateVerificationCode)
                .AddOperation<Verification>(DbOperation.VerifyUser)
                .AddOperation<int>(DbOperation.GetUserById)
                .AddOperation<string>(DbOperation.GetUsersByUsername)
                .AddOperation<Notification>(DbOperation.CreateNotification)
                .AddOperation<Message>(DbOperation.CreateMessage)
                .AddOperation<MessageFilter>(DbOperation.GetMessages)
                .AddOperation<ChatRoom>(DbOperation.CreateChatRooom)
                .AddOperation<int>(DbOperation.GetUserChatrooms)
                .AddOperation<int>(DbOperation.GetChatroomById)
                .AddOperation<ChatRoomMember>(DbOperation.AddMemberToChatroom);

            DataServerBuilder = DataServerBuilder
                .AddHandler(DbOperation.CreateUser, async (input) =>
                {
                    using (var bl = new UsersBL(TycheConfig.ConnectionString))
                    {
                        return await bl.CreateUser(input as User);
                    }
                })
                .AddHandler(DbOperation.CreateVerificationCode, async (input) =>
                {
                    using (var bl = new UsersBL(TycheConfig.ConnectionString))
                    {
                        return await bl.CreateVerificationForUser(input as Verification);
                    }
                })
                .AddHandler(DbOperation.VerifyUser, async (input) =>
                {
                    using (var bl = new UsersBL(TycheConfig.ConnectionString))
                    {
                        return await bl.VerifyUser(input as Verification);
                    }
                })
                .AddHandler(DbOperation.GetUserById, async (input) =>
                {
                    using (var bl = new UsersBL(TycheConfig.ConnectionString))
                    {
                        return await bl.GetUserById((int)input);
                    }
                })
                .AddHandler(DbOperation.GetUsersByUsername, async (input) =>
                {
                    using (var bl = new UsersBL(TycheConfig.ConnectionString))
                    {
                        return await bl.GetUsersByUsername(input as string);
                    }
                })
                .AddHandler(DbOperation.CreateNotification, async (input) =>
                {
                    using (var bl = new BaseBL(TycheConfig.ConnectionString))
                    {
                        return await bl.CreateNotification(input as Notification);
                    }
                })
                .AddHandler(DbOperation.CreateMessage, async (input) =>
                {
                    using (var bl = new MessagesBL(TycheConfig.ConnectionString))
                    {
                        return await bl.CreateMessage(input as Message);
                    }
                })
                .AddHandler(DbOperation.GetMessages, async (input) =>
                {
                    using (var bl = new MessagesBL(TycheConfig.ConnectionString))
                    {
                        return await bl.GetMessages(input as MessageFilter);
                    }
                })
                .AddHandler(DbOperation.CreateChatRooom, async (input) =>
                {
                    using (var bl = new ChatroomsBL(TycheConfig.ConnectionString))
                    {
                        return await bl.CreateChatroom(input as ChatRoom);
                    }
                })
                .AddHandler(DbOperation.GetUserChatrooms, async(input) =>
                {
                    using (var bl = new ChatroomsBL(TycheConfig.ConnectionString))
                    {
                        return await bl.GetChatroomsByUserId((int)input);
                    }
                })
                .AddHandler(DbOperation.GetChatroomById, async(input) =>
                {
                    using (var bl = new ChatroomsBL(TycheConfig.ConnectionString))
                    {
                        return await bl.GetChatroomById((int)input);
                    }
                })
                .AddHandler(DbOperation.AddMemberToChatroom, async (input) =>
                {
                    using (var bl = new ChatroomsBL(TycheConfig.ConnectionString))
                    {
                        return await bl.AddMemberToChatroom(input as ChatRoomMember);
                    }
                });
        }
    }
}