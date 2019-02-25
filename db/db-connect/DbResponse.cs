using System;

namespace DbConnect
{
    /// <summary>
    /// Class for modelling database response
    /// </summary>
    public class DbResponse
    {
        /// <summary>
        /// Gets or sets Response code
        /// </summary>
        public ResponseCode ResponseCode { get; set; }

        /// <summary>
        /// Gets or sets exception
        /// </summary>
        public Exception Exception {get;set;}

        /// <summary>
        /// Gets or sets content
        /// </summary>
        public object Content { get; set; }
    }
}
