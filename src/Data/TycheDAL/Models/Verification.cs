using System;

namespace Tyche.TycheDAL.Models
{
	/// <summary>
	/// Class for modelling Verification entity.
	/// </summary>
	public partial class Verification : DbModel
	{
		/// <summary>
		/// Gets or sets Id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets UserId.
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// Gets or sets Code.
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets Created.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// Gets or sets ValidOffset.
		/// </summary>
		public int ValidOffset { get; set; }
	}
}