/**
 * GNU General Public License Version 3.0, 29 June 2007
 * DataServer
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
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using DbConnect.BL;

namespace DbConnect
{
    /// <summary>
    /// Class for serving data operations via TCP
    /// </summary>
    public class DataServer
    {
        #region fields
        
        /// <summary>
        /// TCP listener for server functionality
        /// </summary>
        private readonly TcpListener server;

        /// <summary>
        /// Operations
        /// </summary>
        private readonly Dictionary<DbOperation, Type> operations; 
        
        /// <summary>
        /// Handlers for database operations
        /// </summary>
        private readonly Dictionary<DbOperation, Func<object, Task<DbResponse>>> handlers;

        /// <summary>
        /// Tasks of data server.
        /// </summary>
        private readonly HashSet<Task> tasks;
        
        /// <summary>
        /// Default maximum data traffic
        /// </summary>
        private const int defaultMaxTraffic = 0x400000;

        /// <summary>
        /// Maximum allowed data that can be recieved.
        /// </summary>
        private int maxAllowedDataToBeRecieved;

        /// <summary>
        /// Boolean value indicating whether stop is needed.
        /// </summary>
        private bool isStopNeeded;
             
        #endregion

        #region properties

        /// <summary>
        /// Gets IP address
        /// </summary>
        public IPAddress IPAddress { get; }

        /// <summary>
        /// Gets port to which server is listening
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets IP endpoint
        /// </summary>
        public IPEndPoint IPEndPoint { get; }

        /// <summary>
        /// Gets the boolean value which indicates whether the data server is running.
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Gets or sets max allowed traffic
        /// </summary>
        public int MaxAllowedDataTraffic
        {
            get
            {
                return this.maxAllowedDataToBeRecieved;
            }
            set
            {
                if (this.maxAllowedDataToBeRecieved == value)
                    return;

                if(value > DataServer.defaultMaxTraffic)
                {
                    throw new ArgumentException("Max Traffic");
                }

                this.maxAllowedDataToBeRecieved = value;
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Creates new instance of <see cref="DataServer"/>
        /// </summary>
        /// <param name="host">IP address of host</param>
        /// <param name="port">Port</param>
        /// <param name="operations">Operations</param>
        /// <param name="handlers">Handlers</param>
        internal DataServer(
            string host,
            string port,
            Dictionary<DbOperation, Type> operations,
            Dictionary<DbOperation, Func<object, Task<DbResponse>>> handlers)
        {
            this.IPAddress = IPAddress.Parse(host);
            this.Port = int.Parse(port);
            this.IPEndPoint = new IPEndPoint(this.IPAddress, this.Port);
            this.server = new TcpListener(this.IPEndPoint);
            this.tasks = new HashSet<Task>();
            this.operations = operations;
            this.handlers = handlers;
            this.maxAllowedDataToBeRecieved = defaultMaxTraffic;
        }

        #endregion constructors

        #region public methods
        
        /// <summary>
        /// Starts Data server.
        /// </summary>
        public void Run()
        {
            this.IsRunning = true;
            this.server.Start();

            try
            {
                while (!this.isStopNeeded)
                {
                    // accepting clients
                    var client = this.server.AcceptTcpClient();

                    // registering task
                    this.RegisterTask(client);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured {0}", ex);
            }
            finally
            {
                this.Stop();
            }
        }

        /// <summary>
        /// Stops the server
        /// </summary>
        public void Stop()
        {
            this.isStopNeeded = true;
            Task.WaitAll(this.tasks.ToArray());
            this.IsRunning = false;
        }

        #endregion

        #region private helper methods
        
        /// <summary>
        /// Registers task for serving new accepted client.
        /// </summary>
        /// <param name="client">TCP client to be served.</param>
        private void RegisterTask(TcpClient client)
        {
            var task = new Task(async () => await this.ServeClient(client));
            this.tasks.RemoveWhere(t => t.IsCompleted);
            this.tasks.Add(task);
            task.Start();
        }

        /// <summary>
        /// Serves client
        /// </summary>
        /// <param name="tcpClient">client</param>
        /// <returns>task</returns>
        private async Task ServeClient(TcpClient tcpClient)
        {
            using (var client = tcpClient)
            {
                using (var stream = client.GetStream())
                {
                    var buffer = new byte[4];
                    var read = 0;

                    // reading frame size
                    read = await stream.ReadAsync(buffer, 0, 4);
                    var frameSize = BitConverter.ToInt32(buffer, 0);
                    
                    // reading DB operation type
                    read = await stream.ReadAsync(buffer, 0, 4);
                    var dbOperationType = (DbOperation)BitConverter.ToInt32(buffer, 0);                    
                    
                    // reading input
                    buffer = new byte[frameSize];
                    read = await stream.ReadAsync(buffer, 0, frameSize);

                    var inputJson = Encoding.Unicode.GetString(buffer);
                    var type = this.operations[dbOperationType];
                    var input = JsonConvert.DeserializeObject(inputJson, type);

                    var handler = this.handlers[dbOperationType];
                    var dbReponse = await handler(input);
                    
                    if(dbReponse.ResponseCode != ResponseCode.Success)
                    {
                        await this.SendErrorResponse(
                            stream,
                            dbReponse.Content as string,
                            dbReponse.ResponseCode);
                        return;
                    }

                    await this.SendSuccessReponse(
                        stream,
                        dbReponse.Content,
                        dbReponse.ResponseCode);
                }
            }
        }

        /// <summary>
        /// Sends success response.
        /// </summary>
        /// <param name="stream">Network stream.</param>
        /// <param name="data">Data</param>
        /// <param name="responseCode">Response code.</param>
        /// <param name="message">Message</param>
        /// <returns>task</returns>
        private async Task SendSuccessReponse(
            NetworkStream stream,
            object data,
            ResponseCode responseCode = ResponseCode.Success,
            string message = Messages.Success)
        {
            await this.SendResponse(stream, message, false, data, responseCode);
        }

        /// <summary>
        /// Sends error response
        /// </summary>
        /// <param name="stream">client stream</param>
        /// <param name="message">message</param>
        /// <param name="responseCode">response code</param>
        /// <returns></returns>
        private async Task SendErrorResponse(
            NetworkStream stream,
            string message = Messages.NoSuchOperation,
            ResponseCode responseCode = ResponseCode.UnknownError)
        {
            await this.SendResponse(stream, message, true, default(object), responseCode);
        }

        /// <summary>
        /// Sends response to the client.
        /// </summary>
        /// <param name="stream">client stream</param>
        /// <param name="message">Response message</param>
        /// <param name="responseCode">Response code</param>
        /// <param name="isError">boolean value indicating whether the reponse is error.</param>
        /// <param name="data">data</param>
        /// <returns>task</returns>
        private async Task SendResponse(
            NetworkStream stream,
            string message = Messages.NoSuchOperation,
            bool isError = true,
            object data = null, 
            ResponseCode responseCode = ResponseCode.UnknownError)
        {
            var response = new Response
            {
                Message = message,
                IsError = isError,
                ResponseCode = responseCode,
                Data = data
            };

            await this.SendResponse(stream, response);
        }

        /// <summary>
        /// Sends Reponse
        /// </summary>
        /// <param name="networkStream">Network Stream</param>
        /// <param name="response">Response</param>
        /// <returns>awaitable task</returns>
        private async Task SendResponse(NetworkStream networkStream, Response response)
        {
            var json = JsonConvert.SerializeObject(response);
            var lengthBytes = BitConverter.GetBytes(json.Length * 2);
            var contentBytes = Encoding.Unicode.GetBytes(json);
            var buffer = lengthBytes
                .Concat(contentBytes)
                .ToArray();

            await networkStream.WriteAsync(buffer, 0, buffer.Length);
        }

        #endregion
    }
}