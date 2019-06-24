using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling MessagesSeenUser entity.
	/// </summary>
	public partial class MessagesSeenUser : DbModel
	{
		/// <summary>
		/// Gets or sets MessageId.
		/// </summary>
		public long MessageId { get; set; }

		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets Seen.
		/// </summary>
		public DateTime Seen { get; set; }
	}
}