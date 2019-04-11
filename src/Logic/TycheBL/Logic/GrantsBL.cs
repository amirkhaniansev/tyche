/**
 * GNU General Public License Version 3.0, 29 June 2007
 * GrantsBL
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tyche.TycheDAL.DataAccess;
using Tyche.TycheDAL.Models;

namespace Tyche.TycheBL.Logic
{
    public class GrantsBL : BaseBL<GrantsDal>
    {
        public GrantsBL(string connectionString = null) : base(connectionString)
        {
            this.Dal = new GrantsDal(this.ConnectionString);
        }

        public async Task<Grant> AddGrant(Grant grant)
        {
            return await this.Dal.CreateGrant(grant);
        }

        public async Task<Grant> GetGrantByKey(string key)
        {
            var grants = await this.Dal
                .GetGrants()
                .Where(grant => grant.Key == key)
                .ToListAsync();

            return grants.FirstOrDefault();
        }

        public async Task<IEnumerable<Grant>> GetGrantsBySubjectId(string subjectId)
        {
            return await this.Dal
                .GetGrants()
                .Where(grant => grant.SubjectId == subjectId)
                .ToListAsync();
        }

        public async Task Remove(string key)
        {
            var isDeleted = await this.Dal.RemoveGrants(grant =>
                   grant.Key == key);

            this.Check(isDeleted);
        }

        public async Task Remove(string subjectId, string clientId)
        {
            var isDeleted = await this.Dal.RemoveGrants(grant =>
                grant.SubjectId == subjectId &&
                grant.ClientId == clientId);

            this.Check(isDeleted);
        }

        public async Task Remove(string subjectId, string clientId, string type)
        {
            var isDeleted = await this.Dal.RemoveGrants(grant =>
                grant.SubjectId == subjectId &&
                grant.ClientId == clientId &&
                grant.Type == type);

            this.Check(isDeleted);
        }

        private void Check(bool isDeleted)
        {
            if (!isDeleted)
                throw new InvalidOperationException("Unable to delete.");
        }
    }
}