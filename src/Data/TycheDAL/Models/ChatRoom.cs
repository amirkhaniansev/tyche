using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling ChatRoom entity.
	/// </summary>
	public partial class ChatRoom : DbModel
	{
		/// <summary>
		/// Gets or sets Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets CreatorId.
		/// </summary>
		public int? CreatorId { get; set; }

		/// <summary>
		/// Gets or sets Name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets Created.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets IsGroup.
		/// </summary>
		public bool IsGroup { get; set; }

		/// <summary>
		/// Gets or sets IsKeyFixed.
		/// </summary>
		public bool IsKeyFixed { get; set; }

		/// <summary>
		/// Gets or sets PictureUrl.
		/// </summary>
		public string PictureUrl { get; set; }
	}
}