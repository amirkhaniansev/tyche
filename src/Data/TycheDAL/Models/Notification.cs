using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling Notification entity.
	/// </summary>
	public partial class Notification : DbModel
	{
		/// <summary>
		/// Gets or sets Id.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets Type.
		/// </summary>
		public int? Type { get; set; }

		/// <summary>
		/// Gets or sets Info.
		/// </summary>
		public string Info { get; set; }

		/// <summary>
		/// Gets or sets ChatRoomId.
		/// </summary>
		public int? ChatRoomId { get; set; }

		/// <summary>
		/// Gets or sets Created.
		/// </summary>
		public DateTime? Created { get; set; }
	}
}