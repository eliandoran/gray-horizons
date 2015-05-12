using System;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents an in-game tank entity.
	/// </summary>
	public class Tank: Entity
	{
		/// <summary>
		/// Represents the orientation of the tank in cardinal points.
		/// </summary>
		public enum Orientation
		{
			North,
			South,
			East,
			West
		}

		/// <summary>
		/// Gets or sets the orientation of the tank.
		/// </summary>
		/// <value>The orientation of the tank.</value>
		public Orientation Facing { get; set; }

		/// <summary>
		/// Gets or sets the health of the tank.
		/// </summary>
		/// <value>The health of the tank.</value>
		public int Health { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Tank"/> class.
		/// </summary>
		public Tank (Map parentMap): base(parentMap)
		{
			Facing = Orientation.North;
		}
	}
}

