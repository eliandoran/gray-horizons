using System;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents an in-game projectile entity.
	/// </summary>
	public class Projectile: Entity
	{
		/// <summary>
		/// Gets or sets the trajectory of projectile.
		/// </summary>
		/// <value>The trajectory of the projectile.</value>
		public Coordinate Trajectory { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Projectile"/> class.
		/// </summary>
		public Projectile (Map parentMap): base(parentMap)
		{
		}

		/// <summary>
		/// Update the state of this projectile.
		/// </summary>
		public void Update()
		{
			Coordinate newLocation = Coordinate.Combine (Location, Trajectory);

			// Check if the projectile has hit the outer border of the map.
			if ((newLocation.X < 0) || (newLocation.X > ParentMap.MapSize.Width) ||
			    (newLocation.Y < 0) || (newLocation.Y > ParentMap.MapSize.Height))
			{
				Dispose ();
				return;
			}

			Location = newLocation;
		}
	}
}

