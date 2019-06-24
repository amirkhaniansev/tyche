using System;

namespace Tyche.TycheDAL.Models.FunctionModels.
{
	/// <summary>
	/// Class for modelling ChatroomUser entity.
	/// </summary>
	public partial class ChatroomUser : DbModel
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
		/// Gets or sets ProfilePictureUrl.
		/// </summary>
		public string ProfilePictureUrl { get; set; }
	}
}