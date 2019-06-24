using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling Grant entity.
	/// </summary>
	public partial class Grant : DbModel
	{
		/// <summary>
		/// Gets or sets Id.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets Key.
		/// </summary>
		public string Key { get; set; }

		/// <summary>
		/// Gets or sets Type.
		/// </summary>
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets SubjectId.
		/// </summary>
		public string SubjectId { get; set; }

		/// <summary>
		/// Gets or sets ClientId.
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		/// Gets or sets CreationTime.
		/// </summary>
		public DateTime CreationTime { get; set; }

		/// <summary>
		/// Gets or sets Expiration.
		/// </summary>
		public DateTime? Expiration { get; set; }

		/// <summary>
		/// Gets or sets Data.
		/// </summary>
		public string Data { get; set; }
	}
}