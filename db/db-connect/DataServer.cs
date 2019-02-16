using System;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AccessCore.Repository;
using Newtonsoft.Json;
using DbConnect.BL;
using DbConnect.Models;

namespace DbConnect
{
    /// <summary>
    /// Class for serving data operations via TCP
    /// </summary>
    public class DataServer
    {
        #region fields

        /// <summary>
        /// Data manager for doing database operations
        /// </summary>
        private readonly DataManager dm;

        /// <summary>
        /// Users Business logic
        /// </summary>
        private readonly UsersBL usersBl;

        /// <summary>
        /// TCP listener for server functionality
        /// </summary>
        private readonly TcpListener server;

        /// <summary>
        /// Operations
        /// </summary>
        private readonly Dictionary<DbOperationType, Type> operations; 
        
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
        /// Constructs new instance of <see cref="DataServer"/>
        /// </summary>
        /// <param name="dm">Data Manager</param>
        /// <param name="address">address</param>
        /// <param name="port">port</param>
        /// /// <exception cref="ArgumentNullException">
        /// Throws if any of paramaters is not correct.
        /// </exception>
        public DataServer(DataManager dm, string address, string port)
            : this(dm, IPAddress.Parse(address), int.Parse(port))
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="DataManager"/>
        /// </summary>
        /// <param name="dm">Data manager</param>
        /// <param name="iPEndPoint">IP endpoint</param>
        public DataServer(DataManager dm, IPEndPoint iPEndPoint)
            : this(dm, iPEndPoint.Address, iPEndPoint.Port)
        {
        }

        /// <summary>
        /// Creates new instance of <see cref="DataManager"/>
        /// </summary>
        /// <param name="dm">Data Manager</param>
        /// <param name="address">IP address of server.</param>
        /// <param name="port">Port of server.</param>
        /// <exception cref="ArgumentNullException">
        /// Throws if any of paramaters is not correct.
        /// </exception>
        public DataServer(DataManager dm, IPAddress address, int port)
        {
            if(dm == null)
            {
                throw new ArgumentNullException("Data Manager");
            }

            if(address == null)
            {
                throw new ArgumentNullException("IP Address");
            }

            if(port < 1000)
            {
                throw new ArgumentNullException("Port");
            }

            this.dm = dm;
            this.IPAddress = address;
            this.Port = port;
            this.maxAllowedDataToBeRecieved = DataServer.defaultMaxTraffic;
            this.IPEndPoint = new IPEndPoint(address, port);
            this.server = new TcpListener(address, port);
            this.tasks = new HashSet<Task>();
            this.operations = new Dictionary<DbOperationType, Type>();
            this.usersBl = new UsersBL(dm);
        }

        #endregion constructors

        #region public methods

        /// <summary>
        /// Adds data operation
        /// </summary>
        /// <typeparam name="TInput">Type of input</typeparam>
        /// <param name="dbOperationType">Database operation type</param>
        /// /// <exception cref="ArgumentException">
        /// Throws if database operation type is already added.
        /// </exception>
        /// <returns>data server</returns>
        public DataServer AddDataOperation<TInput>(DbOperationType dbOperationType)
        {
            return this.AddDataOperation(dbOperationType, typeof(TInput));
        }

        /// <summary>
        /// Adds data operation
        /// </summary>
        /// <param name="dbOperationType">Database operation type</param>
        /// <param name="type">Type of input</param>
        /// <exception cref="ArgumentException">
        /// Throws if database operation type is already added.
        /// </exception>
        /// <returns>data server</returns>
        public DataServer AddDataOperation(DbOperationType dbOperationType, Type type)
        {
            if(this.operations.ContainsKey(dbOperationType))
            {
                throw new ArgumentException("Database Operation Type.");
            }

            this.operations.Add(dbOperationType, type);
            return this;
        }

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

                    // reading entity type
                    read = await stream.ReadAsync(buffer, 0, 4);
                    var entityType = (BlType)BitConverter.ToInt32(buffer, 0);

                    // reading DB operation type
                    read = await stream.ReadAsync(buffer, 0, 4);
                    var dbOperationType = (DbOperationType)BitConverter.ToInt32(buffer, 0);                    
                    
                    // reading input
                    buffer = new byte[frameSize];
                    read = await stream.ReadAsync(buffer, 0, frameSize);

                    var inputJson = Encoding.Unicode.GetString(buffer);
                    var type = this.operations[dbOperationType];
                    var input = JsonConvert.DeserializeObject(inputJson, type);

                    var result = await this.usersBl.CreateUser(input as User);

                    await this.SendSuccessReponse(stream, result);
                }
            }
        }

        /// <summary>
        /// Sends success response.
        /// </summary>
        /// <typeparam name="TData">Type of data.</typeparam>
        /// <param name="stream">Network stream.</param>
        /// <param name="data">Data</param>
        /// <param name="responseCode">Response code.</param>
        /// <param name="message">Message</param>
        /// <returns>task</returns>
        private async Task SendSuccessReponse<TData>(
            NetworkStream stream,
            TData data,
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
        /// <typeparam name="T">Type of response entity.</typeparam>
        /// <param name="stream">client stream</param>
        /// <param name="message">Response message</param>
        /// <param name="responseCode">Response code</param>
        /// <param name="isError">boolean value indicating whether the reponse is error.</param>
        /// <param name="data">data</param>
        /// <returns>task</returns>
        private async Task SendResponse<T>(
            NetworkStream stream,
            string message = Messages.NoSuchOperation,
            bool isError = true,
            T data = default(T), 
            ResponseCode responseCode = ResponseCode.UnknownError)
        {
            var response = new Response<T>
            {
                Message = message,
                IsError = isError,
                ResponseCode = responseCode,
                Data = data
            };

            var json = JsonConvert.SerializeObject(response);
            var lengthBytes = BitConverter.GetBytes(json.Length * 2);
            var contentBytes = Encoding.Unicode.GetBytes(json);
            var buffer = lengthBytes
                .Concat(contentBytes)
                .ToArray();

            await stream.WriteAsync(buffer, 0, buffer.Length);
        }

        #endregion
    }
}