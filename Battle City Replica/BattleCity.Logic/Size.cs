using System;

namespace BattleCity.Logic
{
	public class Size
	{
		/// <summary>
		/// Gets or sets the width value.
		/// </summary>
		/// <value>The width value.</value>
		public int Width { get; set; }

		/// <summary>
		/// Gets or sets the height value.
		/// </summary>
		/// <value>The height value.</value>
		public int Height { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Size"/> class.
		/// </summary>
		/// <param name="width">The width value of this size.</param>
		/// <param name="height">The height value of this size.</param>
		public Size(int width, int height)
		{
			Width 	= width;
			Height 	= height;
		}
	}
}