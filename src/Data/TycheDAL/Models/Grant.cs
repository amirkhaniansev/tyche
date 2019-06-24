/**
 * GNU General Public License Version 3.0, 29 June 2007
 * Grant
 * Copyright (C) <2019>
 *      Authors: <amirkhaniansev>  <amirkhanyan.sevak@gmail.com>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * Full notice : https://github.com/amirkhaniansev/tyche/tree/master/LICENSE
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
**/

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