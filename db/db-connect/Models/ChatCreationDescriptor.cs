namespace DbConnect.Models
{
    /// <summary>
    /// Model for describing chat creation
    /// </summary>
    public class ChatCreationDescriptor
    {
        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets info if chat is group chat
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// Gets or sets picture url
        /// </summary>
        public string PictureUrl { get; set; }
    }
}