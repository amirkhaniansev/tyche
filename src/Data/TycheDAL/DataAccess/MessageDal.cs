/**
 * GNU General Public License Version 3.0, 29 June 2007
 * MessageDal
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
using System.Linq;
using System.Threading.Tasks;
using Tyche.TycheDAL.Context;
using Tyche.TycheDAL.Models;

namespace Tyche.TycheDAL.DataAccess
{
    public class MessageDal : BaseDal
    {
        public MessageDal(string connectionString, TycheContext context = null)
            : base(connectionString, context)
        {
        }

        public async Task<Message> CreateMessage(Message message, bool saveAfterAdding = true)
        {
            return await this.AddEntity(message, saveAfterAdding);
        }

        public IQueryable<Message> GetMessages()
        {
            return this.GetMessages(m => true);
        }

        private IQueryable<Message> GetMessages(Predicate<Message> filter)
        {
            return this.Db.Messages
                .AsQueryable()
                .Where(message => filter.Invoke(message));
        }
    }
}
