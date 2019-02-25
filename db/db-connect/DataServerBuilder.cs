using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

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