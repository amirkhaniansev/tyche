using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling Message entity.
	/// </summary>
	public partial class Message : DbModel
	{
		/// <summary>
		/// Gets or sets Id.
		/// </summary>
		public long Id { get; set; }

		/// <summary>
		/// Gets or sets From.
		/// </summary>
		public int? From { get; set; }

		/// <summary>
		/// Gets or sets To.
		/// </summary>
		public int? To { get; set; }

		/// <summary>
		/// Gets or sets Header.
		/// </summary>
		public string Header { get; set; }

		/// <summary>
		/// Gets or sets Text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets Created.
		/// </summary>
		public DateTime? Created { get; set; }
	}
}