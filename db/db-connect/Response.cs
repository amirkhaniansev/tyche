namespace DbConnect
{
    /// <summary>
    /// Class for response
    /// </summary>
    /// <typeparam name="T">Type of Data</typeparam>
    internal class Response<T>
    {
        /// <summary>
        /// Gets or sets response code
        /// </summary>
        public ResponseCode ResponseCode { get; set; }
        
        /// <summary>
        /// Gets or sets boolean value which indicates whether
        /// the response contains error.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Gets or sets message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets data
        /// </summary>
        public T Data { get; set; }
    }
}