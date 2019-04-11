/**
 * GNU General Public License Version 3.0, 29 June 2007
 * ChatroomsDal
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
using System.Threading.Tasks;
using Tyche.TycheDAL.Context;
using Tyche.TycheDAL.Models;

namespace Tyche.TycheDAL.DataAccess
{
    public class ChatroomsDal : BaseDal
    {
        public ChatroomsDal(string connectionString, TycheContext context = null) 
            : base(connectionString, context)
        {
        }

        public async Task<ChatRoom> CreateChatroom(ChatRoom chatRoom, bool saveAfterChanges)
        {
            return await this.AddEntity(chatRoom, saveAfterChanges);
        }

        public IQueryable<ChatRoom> GetChatrooms()
        {
            return this.Db.ChatRooms.AsQueryable();
        }

        public IQueryable<int> GetChatroomMembersIds(int chatroomId)
        {
            return this.GetChatRoomMembers(chatroomId)
                    .Select(crm => crm.UserId);
        }

        public IQueryable<ChatRoomMember> GetChatRoomMembers(int chatroomId)
        {
            return this.Db
                   .ChatroomMembers
                   .AsQueryable()
                   .Where(crm => crm.UserId == chatroomId);
        }

        public async Task<bool> DeleteChatroom(ChatRoom chatRoom)
        {
            var entry = this.Db.ChatRooms.Remove(chatRoom);

            return await this.SaveChanges();
        }
    }
}
