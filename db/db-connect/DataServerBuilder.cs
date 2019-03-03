/**
 * GNU General Public License Version 3.0, 29 June 2007
 * DataServerBuilder
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
using System.Collections.Generic;
using System.Threading.Tasks;
using TycheBL;

namespace DbConnect
{
    /// <summary>
    /// Builder class for DataServer
    /// </summary>
    public class DataServerBuilder
    {
        #region fields

        /// <summary>
        /// Operations
        /// </summary>
        private readonly Dictionary<DbOperation, Type> operations;

        /// <summary>
        /// Handlers for database operations
        /// </summary>
        private readonly Dictionary<DbOperation, Func<object, Task<DbResponse>>> handlers;

        /// <summary>
        /// IP address
        /// </summary>
        private string ip;

        /// <summary>
        /// Port
        /// </summary>
        private string port;

        #endregion

        #region constructors

        /// <summary>
        /// Creates new instance of <see cref="DataServerBuilder"/>
        /// </summary>
        public DataServerBuilder()
        {
            this.operations = new Dictionary<DbOperation, Type>();
            this.handlers = new Dictionary<DbOperation, Func<object, Task<DbResponse>>>();
        }

        #endregion

        #region public methods

        /// <summary>
        /// Assigns IP address
        /// </summary>
        /// <param name="ip">IP address</param>
        /// <returns>data server builder</returns>
        public DataServerBuilder AssignIp(string ip)
        {
            this.ip = ip;
            return this;
        }

        /// <summary>
        /// Assigns port
        /// </summary>
        /// <param name="port">Port</param>
        /// <returns>data server builder</returns>
        public DataServerBuilder AssignPort(string port)
        {
            this.port = port;
            return this;
        }

        /// <summary>
        /// Adds data server operation
        /// </summary>
        /// <typeparam name="TInput">Type of operation input parameters</typeparam>
        /// <param name="dbOperation">Database operation type</param>
        /// <returns>data serer builder</returns>
        public DataServerBuilder AddOperation<TInput>(DbOperation dbOperation)
        {
            if (this.operations.ContainsKey(dbOperation))
                throw new ArgumentException("Operation exists.");

            this.operations.Add(dbOperation, typeof(TInput));
            return this;
        }

        /// <summary>
        /// Adds handler for database operations
        /// </summary>
        /// <param name="dbOperation">Database operation</param>
        /// <param name="handler">Hadnler</param>
        /// <returns>data server builder</returns>
        public DataServerBuilder AddHandler(DbOperation dbOperation, Func<object, Task<DbResponse>> handler)
        {
            if (!this.operations.ContainsKey(dbOperation))
                throw new InvalidOperationException("No such operation");

            if (this.handlers.ContainsKey(dbOperation))
                throw new ArgumentException("Handler exists.");

            this.handlers.Add(dbOperation, handler);
            return this;
        }

        /// <summary>
        /// Builds data server.
        /// </summary>
        /// <returns>data server built with the builder parameters</returns>
        public DataServer Build()
        {
            if (string.IsNullOrEmpty(this.ip))
                throw new InvalidOperationException("Ip");

            if (string.IsNullOrEmpty(this.port))
                throw new InvalidOperationException("Port");

            var operationTypes = Enum.GetValues(typeof(DbOperation))
                .Cast<DbOperation>();

            if (!operationTypes.All(opType => this.operations.ContainsKey(opType)))
                throw new InvalidOperationException("Operations");

            if (!operationTypes.All(opType => this.handlers.ContainsKey(opType)))
                throw new InvalidOperationException("Handlers");

            return new DataServer(
                this.ip,
                this.port,
                this.operations,
                this.handlers);
        }
        
        #endregion 
    }
}