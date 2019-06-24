using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling NotificationAssignment entity.
	/// </summary>
	public partial class NotificationAssignment : DbModel
	{
		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets NotificationId.
		/// </summary>
		public long NotificationId { get; set; }

		/// <summary>
		/// Gets or sets IsSeen.
		/// </summary>
		public bool? IsSeen { get; set; }
	}
}