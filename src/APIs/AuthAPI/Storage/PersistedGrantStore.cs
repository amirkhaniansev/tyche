/**
 * GNU General Public License Version 3.0, 29 June 2007
 * PersistedGrantStore
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
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Tyche.TycheBL.Logic;
using Tyche.TycheDAL.Models;
using Tyche.TycheApiUtilities;

namespace Tyche.AuthAPI.Storage
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            using (var bl = this.CreateBL())
            {
                var dbGrants = await bl.GetGrantsBySubjectId(subjectId);

                return dbGrants.Select(PersistedGrantStore.ConvertGrantToPersistedGrant);
            }
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            using (var bl = this.CreateBL())
            {
                var grant = await bl.GetGrantByKey(key);

                return PersistedGrantStore.ConvertGrantToPersistedGrant(grant);
            }
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            using (var bl = this.CreateBL())
            {
                return bl.Remove(subjectId, clientId);
            }
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            using (var bl = this.CreateBL())
            {
                return bl.Remove(subjectId, clientId, type);
            }
        }

        public Task RemoveAsync(string key)
        {
            using (var bl = this.CreateBL())
            {
                return bl.Remove(key);
            }
        }

        public async Task StoreAsync(PersistedGrant grant)
        {
            using (var bl = this.CreateBL())
            {
                var addedGrant = await bl.AddGrant(
                    PersistedGrantStore.ConvertPersistedGrantToGrant(grant));

                if (addedGrant.Id == 0)
                    throw new InvalidOperationException("Grant");
            }
        }

        private GrantsBL CreateBL()
        {
            return new GrantsBL(App.ConnectionString);
        }

        private static PersistedGrant ConvertGrantToPersistedGrant(Grant grant)
        {
            return new PersistedGrant
            {
                ClientId = grant.ClientId,
                SubjectId = grant.SubjectId,
                CreationTime = grant.CreationTime,
                Data = grant.Data,
                Expiration = grant.Expiration,
                Key = grant.Key,
                Type = grant.Type
            };
        }

        private static Grant ConvertPersistedGrantToGrant(PersistedGrant persistedGrant)
        {
            return new Grant
            {
                ClientId = persistedGrant.ClientId,
                SubjectId = persistedGrant.SubjectId,
                CreationTime = persistedGrant.CreationTime,
                Data = persistedGrant.Data,
                Expiration = persistedGrant.Expiration,
                Key = persistedGrant.Key,
                Type = persistedGrant.Type
            };
        }
    }
}