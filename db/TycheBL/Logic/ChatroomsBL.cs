/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ChatroomsBL
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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TycheBL.Models;

namespace TycheBL.Logic
{
    /// <summary>
    /// Business logic for chatrooms
    /// </summary>
    public class ChatroomsBL : BaseBL
    {
        /// <summary>
        /// Creates new instance of <see cref="ChatroomsBL"/>
        /// </summary>
        /// <param name="connectionString">database connection string</param>
        public ChatroomsBL(string connectionString) : base(connectionString, BlType.ChatroomsBL)
        {
        }

        /// <summary>
        /// Creates chatroom asynchronously
        /// </summary>
        /// <param name="chatRoom">chatroom</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> CreateChatroom(ChatRoom chatRoom)
        {
            try
            {
                if (!this.Db.Users.Any(u => u.Id == chatRoom.CreatorId))
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.UserNotExist,
                        Messages.UserNotExists);
                }

                if (this.Db.ChatRooms.Any(cr => cr.CreatorId == chatRoom.CreatorId && cr.Name == chatRoom.Name))
                {
                    return Helper.ConstructDbResponse(
                        ResponseCode.ChatroomExists,
                        Messages.ChatroomExists);
                }
                
                var entity = await this.Db.ChatRooms.AddAsync(chatRoom);

                if (!await this.SaveChanges())
                {
                    return Helper.ConstructDbError();
                }

                var crm = new ChatRoomMember
                {
                    UserId = chatRoom.CreatorId.GetValueOrDefault(),
                    ChatRoomId = chatRoom.Id,
                };

                await this.Db.ChatroomMembers.AddAsync(crm);
                await this.Db.SaveChangesAsync();

                return Helper.ConstructDbResponse(ResponseCode.Success, entity.Entity);
            }
            catch(Exception ex)
            {
                return Helper.ConstructDbError(ex);
            }
        }

        /// <summary>
        /// Adds member to chatroom
        /// </summary>
        /// <param name="chatRoomMember">chatroom member</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> AddMemberToChatroom(ChatRoomMember chatRoomMember)
        {
            try
            {
                if (!this.Db.Users.Any(u => u.Id == chatRoomMember.UserId))
                {
                    return Helper.ConstructDbResponse(ResponseCode.UserNotExist);
                }

                if (!this.Db.ChatRooms.Any(cr => cr.Id == chatRoomMember.ChatRoomId))
                {
                    return Helper.ConstructDbResponse(ResponseCode.ChatroomNotExist);
                }
                
                if (this.Db.ChatroomMembers.Contains(chatRoomMember))
                {
                    return Helper.ConstructDbResponse(ResponseCode.MemberIsAlreadyInChatroom);
                }

                var entity = await this.Db.ChatroomMembers.AddAsync(chatRoomMember);
                var isSaved = await this.SaveChanges();

                if (!isSaved)
                {
                    return Helper.ConstructDbError();
                }

                var members = await this.Db
                    .ChatroomMembers
                    .Where(crm => crm.ChatRoomId == chatRoomMember.ChatRoomId)
                    .Select(crm => crm.UserId)
                    .ToListAsync();

                var notification = Helper.ConstructNotification(
                    NotificationType.NewUserToChatroom, 
                    members,
                    Messages.NewMemberIsAddedToChatroom);
                var response = await this.CreateNotification(notification);

                return response;
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbError(ex);
            }
        }
        
        /// <summary>
        /// Gets user chatrooms
        /// </summary>
        /// <param name="userId">user ID</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> GetChatroomsByUserId(int userId)
        {
            try
            {
                if (!this.Db.Users.Any(u => u.Id == userId))
                {
                    return Helper.ConstructDbResponse(ResponseCode.UserNotExist);
                }

                var chats = this.Db.ChatRooms;
                var chatrooms = await this.Db
                    .ChatroomMembers
                    .Where(crm => crm.UserId == userId)
                    .Join(chats, crm => crm.ChatRoomId, cr => cr.Id, (crm, cr) => cr)
                    .ToListAsync();

                if (chatrooms == null || !chatrooms.Any())
                {
                    return Helper.ConstructDbResponse(ResponseCode.NoContent);
                }

                return Helper.ConstructDbResponse(ResponseCode.Success, chatrooms);
            }
            catch(Exception ex)
            {
                return Helper.ConstructDbError(ex);
            }
        }

        /// <summary>
        /// Gets chatroom by ID
        /// </summary>
        /// <param name="chatroomId">chatroom ID</param>
        /// <returns>database response</returns>
        public async Task<DbResponse> GetChatroomById(int chatroomId)
        {
            try
            {
                var chatroom = await this.Db.ChatRooms.FindAsync(chatroomId);
                if (chatroom == null)
                {
                    return Helper.ConstructDbResponse(ResponseCode.ChatroomNotExist);
                }

                var users = this.Db.Users;
                var chatroomUsers = await this.Db
                    .ChatroomMembers
                    .Where(crm => crm.ChatRoomId == chatroomId)
                    .Join(users, crm => crm.UserId, u => u.Id,
                    (crm, u) =>
                        new User
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Username = u.Username,
                            Email = u.Email,
                            ProfilePictureUrl = u.ProfilePictureUrl
                        })
                    .ToListAsync();
                
                if (chatroomUsers == null || !chatroomUsers.Any())
                {
                    return Helper.ConstructDbResponse(ResponseCode.NoContent);
                }

                chatroom.Members = chatroomUsers;
                return Helper.ConstructDbResponse(ResponseCode.Success, chatroom);
            }
            catch (Exception ex)
            {
                return Helper.ConstructDbError(ex);
            }
        }
    }
}