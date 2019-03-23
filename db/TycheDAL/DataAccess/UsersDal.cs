/**
 * GNU General Public License Version 3.0, 29 June 2007
 * UsersDal
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
using TycheDAL.Context;
using TycheDAL.Models;

namespace TycheDAL.DataAccess
{
    public class UsersDal : BaseDal
    {
        public UsersDal(string connectionString, TycheContext context = null) 
            : base(connectionString, context)
        {
        }

        public bool Exists(Predicate<User> userExistancePredicate)
        {
            return this.Db.Users.Any(u => userExistancePredicate(u));
        }

        public async Task<User> CreateUser(User user, bool saveAfterAdding = true)
        {
            return await this.AddEntity(user, saveAfterAdding);
        }

        public async Task<User> GetUserById(int userId)
        {
            return await this.Db.Users.FindAsync(userId);
        }

        public User GetUserByUsername(string username)
        {
            return this.Db.Users.FirstOrDefault(u => u.Username == username);
        }

        public IQueryable<User> GetUsersByIds(params int[] userIds)
        {
            return this.Db.Users.AsQueryable().Where(user => userIds.Contains(user.Id));
        }

        public IQueryable<User> GetUsersByUsername(string username)
        {
            var usersQuery = this.Db.Users.AsQueryable();

            return usersQuery.Where(u => u.Username.Contains(username));
        }
    }
}