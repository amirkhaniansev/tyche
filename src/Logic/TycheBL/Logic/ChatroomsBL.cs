/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ChatroomsBL
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

using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tyche.TycheDAL.Models;
using Tyche.TycheDAL.DataAccess;
using Tyche.TycheBL.Models;
using Tyche.TycheBL.Constants;

namespace Tyche.TycheBL.Logic
{
    public class ChatroomsBL : BaseBL<ChatroomsDal>
    {
        public ChatroomsBL(string connectionString = null) : base(connectionString)
        {
        }

        public async Task<List<Chat>> GetUserChats(int userId, bool includeMembers = false)
        {
            var usersDal = new UsersDal(this.ConnectionString, this.Dal.Db);
            var user = await usersDal.GetUserById(userId);
            if (user == null)
                return null;

            var userChatroomIds = await this.Dal.Db
                .ChatroomMembers
                .AsQueryable()
                .Where(crm => crm.UserId == userId)
                .Select(crm => crm.ChatRoomId)
                .ToListAsync();

            if (userChatroomIds == null || userChatroomIds.Count == 0)
                return new List<Chat>();

            var chats = new List<Chat>(userChatroomIds.Count);
            var chatroom = default(ChatRoom);
            var creator = default(User);
            var memberIds = default(int[]);
            var members = default(List<User>);

            foreach (var userChatroomId in userChatroomIds)
            {
                chatroom = await this.Dal.Db.ChatRooms.FindAsync(userChatroomId);  
                creator = await usersDal.GetUserById(chatroom.CreatorId.GetValueOrDefault());

                if (includeMembers)
                {
                    memberIds = await this.Dal
                        .GetChatroomMembersIds(chatroom.Id)
                        .ToArrayAsync();

                    members = await usersDal
                        .GetUsersByIds(memberIds)
                        .Select(u => new User
                        {
                            Id = u.Id,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Username = u.Username,
                            ProfilePictureUrl = u.ProfilePictureUrl
                        })
                        .ToListAsync();
                }

                chats.Add(new Chat
                {
                    Created = chatroom.Created,
                    CreatorId = chatroom.CreatorId,
                    Creator = creator,
                    Id = chatroom.Id,
                    IsGroup = chatroom.IsGroup,
                    IsKeyFixed = chatroom.IsKeyFixed,
                    Name = chatroom.Name,
                    PictureUrl = chatroom.PictureUrl,
                    Members = members
                });
            }

            return chats;
        }
    }
}
