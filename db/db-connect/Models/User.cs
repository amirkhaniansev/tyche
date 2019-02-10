namespace DbConnect.Models
{
    /// <summary>
    /// Model for user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets profile picture url
        /// </summary>
        public string ProfilePictureUrl { get; set; }

        /// <summary>
        /// Gets or sets password hash
        /// </summary>
        public string PasswordHash { get; set; }
    }
}