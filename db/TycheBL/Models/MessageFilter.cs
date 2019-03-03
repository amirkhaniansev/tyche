using System;

namespace TycheBL.Models
{
    /// <summary>
    /// Model for filtering messages
    /// </summary>
    public class MessageFilter
    {
        /// <summary>
        /// Gets or sets from date
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Gets or sets to date
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Gets or sets chatroom ID
        /// </summary>
        public int ChatroomId { get; set; }
    }
}