using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling User entity.
	/// </summary>
	public partial class User : DbModel
	{
		/// <summary>
		/// Gets or sets Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets FirstName.
		/// </summary>
		public string FirstName { get; set; }

		/// <summary>
		/// Gets or sets LastName.
		/// </summary>
		public string LastName { get; set; }

		/// <summary>
		/// Gets or sets Username.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// Gets or sets Email.
		/// </summary>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets ProfilePictureUrl.
		/// </summary>
		public string ProfilePictureUrl { get; set; }

		/// <summary>
		/// Gets or sets PasswordHash.
		/// </summary>
		public string PasswordHash { get; set; }

		/// <summary>
		/// Gets or sets IsVerified.
		/// </summary>
		public bool IsVerified { get; set; }

		/// <summary>
		/// Gets or sets IsActive.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets Created.
		/// </summary>
		public DateTime Created { get; set; }
	}
}