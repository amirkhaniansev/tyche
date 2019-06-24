using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling ChatRoomMember entity.
	/// </summary>
	public partial class ChatRoomMember : DbModel
	{
		/// <summary>
		/// Gets or sets ChatRoomId.
		/// </summary>
		public int ChatRoomId { get; set; }

		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets FixedHeader.
		/// </summary>
		public string FixedHeader { get; set; }

		/// <summary>
		/// Gets or sets JoinedDate.
		/// </summary>
		public DateTime JoinedDate { get; set; }
	}
}