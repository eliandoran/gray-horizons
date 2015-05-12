using System;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents an in-game coordinate that is relative to a map.
	/// </summary>
	public class Coordinate
	{
		/// <summary>
		/// Gets or sets the horizontal value (X) of this coordinate.
		/// </summary>
		/// <value>The X value.</value>
		public int X { get; set; }

		/// <summary>
		/// Gets or sets the vertical value (Y) of this coordinate..
		/// </summary>
		/// <value>The Y value.</value>
		public int Y { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Coordinate"/> class with the specified X and Y values.
		/// </summary>
		/// <param name="x">The X coordinate.</param>
		/// <param name="y">The Y coordinate.</param>
		public Coordinate(int x, int y)
		{
			X = x;
			Y = y;
		}

		/// <summary>
		/// Determines whether the specified coordinate is equal to the current coordinate.
		/// </summary>
		/// <returns>true if the specified coordinate is equal to the current coordinate; otherwise, false.</returns>
		/// <filterpriority>2</filterpriority>
		/// <param name="compareWith">The <see cref="BattleCity.Logic.Coordinate"/> to compare with the current <see cref="BattleCity.Logic.Coordinate"/>.</param>
		public bool Equals(Coordinate compareWith)
		{
			return X == compareWith.X && Y == compareWith.Y;
		}

		/// <summary>
		/// Adds two coordinates together.
		/// </summary>
		/// <param name="first">The first coordinate.</param>
		/// <param name="second">The second coordinate.</param>
		public static Coordinate Combine(Coordinate first, Coordinate second)
		{
			return new Coordinate (
				first.X + second.X,
				first.Y + second.Y);
		}
	}
}

