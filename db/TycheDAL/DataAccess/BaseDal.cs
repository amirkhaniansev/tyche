/**
 * GNU General Public License Version 3.0, 29 June 2007
 * BaseDal
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
using System.Threading.Tasks;
using TycheDAL.Context;

namespace TycheDAL.DataAccess
{
    public class BaseDal : IDisposable
    {
        private readonly string connectionString;

        private TycheContext db;

        public string DbConnectionString => this.connectionString;

        public TycheContext Db => this.db;

        public BaseDal(string connectionString, TycheContext context = null)
        {
            this.connectionString = connectionString;

            this.InitializeContext(context);
        }

        public async Task<bool> SaveChanges()
        {
            using (var transaction = await this.db.Database.BeginTransactionAsync())
            {
                try
                {
                    await this.db.SaveChangesAsync();
                    transaction.Commit();
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public void Dispose()
        {
            this.db.Dispose();
        }
        
        protected virtual async Task<TEntity> AddEntity<TEntity>(TEntity entity, bool saveAfterAdding = true)
            where TEntity : class
        {
            var entry = await this.db.AddAsync(entity);

            if (saveAfterAdding && await this.SaveChanges())
                return entry.Entity;

            return null;
        }

        private void InitializeContext(TycheContext context)
        {
            if (context == null)
                context = new TycheContext(connectionString);
            this.db = context;
        }
    }
}