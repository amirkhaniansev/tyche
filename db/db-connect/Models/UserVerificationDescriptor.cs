namespace DbConnect.Models
{
    /// <summary>
    /// Model for describing user verifications
    /// </summary>
    public class UserVerificationDescriptor
    {
        /// <summary>
        /// Gets or sets user id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets code
        /// </summary>
        public string Code { get; set; }
    }
}