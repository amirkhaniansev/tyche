/**
 * GNU General Public License Version 3.0, 29 June 2007
 * DataClient
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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DbConnectClient
{
    /// <summary>
    /// Client class for connecting Data server
    /// </summary>
    public class DataClient
    {
        /// <summary>
        /// IP address of Data Server
        /// </summary>
        private readonly IPAddress dataServerAddress;

        /// <summary>
        /// Port to which Data Server is listening
        /// </summary>
        private readonly int port;

        /// <summary>
        /// Creat new instance of <see cref="DataClient"/>
        /// </summary>
        /// <param name="iPAddress">IP address.</param>
        /// <param name="port">port</param>
        public DataClient(IPAddress iPAddress, int port)
        {
            this.dataServerAddress = iPAddress;
            this.port = port;
        }

        /// <summary>
        /// Sends response to data server asynchronously.
        /// </summary>
        /// <typeparam name="TInput">Type of input.</typeparam>
        /// <param name="request">Data Server Request.</param>
        /// <returns>response from DbConnect service.</returns>
        public async Task<Response> SendRequestAsync<TInput>(Request<TInput> request)
        {
            if (request == null)
                throw new ArgumentNullException("Request");

            try
            {
                var buffer = await this.GetRequestBufferAsync(request);

                using (var tcpClient = new TcpClient())
                {
                    await tcpClient.ConnectAsync(this.dataServerAddress, this.port);

                    using (var stream = tcpClient.GetStream())
                    {
                        await stream.WriteAsync(buffer, 0, buffer.Length);

                        var outputLengthBytes = new byte[4];
                        var read = await stream.ReadAsync(buffer, 0, 4);
                        var length = BitConverter.ToInt32(outputLengthBytes);

                        var output = new byte[length];
                        var outputRead = await stream.ReadAsync(output, 0, length);
                        var json = Encoding.Unicode.GetString(output);
                        var response = JsonConvert.DeserializeObject<Response>(json);

                        return response;
                    }
                }
            }
            catch (Exception)
            {
                return new Response
                {
                    ResponseCode = ResponseCode.InternalError,
                    IsError = true
                };
            }
        }

        /// <summary>
        /// Gets request serialization in bytes asynchornously.
        /// </summary>
        /// <typeparam name="TInput">Type of input.</typeparam>
        /// <param name="request">request</param>
        /// <returns>task doing serialization that can be awaited</returns>
        private Task<byte[]> GetRequestBufferAsync<TInput>(Request<TInput> request)
        {
            return Task.Run(() => this.GetRequestBuffer(request));
        }

        /// <summary>
        /// Gets request serialization in bytes.
        /// </summary>
        /// <typeparam name="TInput">Type of input.</typeparam>
        /// <param name="request">Request</param>
        /// <returns>byte representation of request.</returns>
        private byte[] GetRequestBuffer<TInput>(Request<TInput> request)
        {
            // serializing operation type
            var operation = (int)request.Operation;
            var bytesOfOperation = BitConverter.GetBytes(operation);

            // serializing input
            var json = JsonConvert.SerializeObject(request.Input);
            var bytesOfJson = Encoding.Unicode.GetBytes(json);

            // serializng length
            var lengthOfFrame = bytesOfJson.Length;
            var bytesOfLength = BitConverter.GetBytes(lengthOfFrame);

            var bufferLength = bytesOfOperation.Length + bytesOfLength.Length + lengthOfFrame;
            var buffer = new byte[bufferLength];

            Array.Copy(bytesOfLength, 0, buffer, 0, bytesOfLength.Length);
            Array.Copy(bytesOfOperation, 0, buffer, bytesOfLength.Length, bytesOfOperation.Length);
            Array.Copy(bytesOfJson, 0, buffer, bytesOfLength.Length + bytesOfOperation.Length, bytesOfJson.Length);

            return buffer;
        }
    }
}