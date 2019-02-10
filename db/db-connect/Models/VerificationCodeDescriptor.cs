namespace DbConnect.Models
{
    /// <summary>
    /// Class for describing verification code creation
    /// </summary>
    public class VerificationCodeDescriptor
    {
        /// <summary>
        /// Gets or sets user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets valid offset
        /// </summary>
        public int ValidOffset { get; set; }
    }
}