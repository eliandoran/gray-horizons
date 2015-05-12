using System;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents a wall on the map.
	/// </summary>
	public class Wall: StaticObject
	{
		/// <summary>
		/// Gets a matrix of segments of the wall, where True means it is exists, False otherwise.
		/// </summary>
		/// <value>The segments matrix.</value>
		public bool[,] Segments { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Wall"/> class with all segments intact.
		/// </summary>
		/// <param name="parentMap">The map which contains this wall.</param>
		public Wall (Map parentMap): base(parentMap)
		{
			Segments = new bool[2, 2] {
				{ true, true },
				{ true, true }
			};
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Wall"/> class.
		/// </summary>
		/// <param name="parentMap">The map which contains this wall.</param>
		/// <param name="segments">A matrix containing the segments integrity of this wall.</param>
		public Wall (Map parentMap, bool[,] segments) : base (parentMap)
		{
			Segments = segments;
		}
	}
}

