using System;
using System.Windows;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents an immovable object on the in-game map.
	/// </summary>
	public class StaticObject
	{
		readonly Map parentMap;

		/// <summary>
		/// Gets the map which contains this entity.
		/// </summary>
		/// <value>The parent map.</value>
		public Map ParentMap {
			get {
				return parentMap;
			}
		}

		/// <summary>
		/// Gets or sets the location of this object relative to its parent map.
		/// </summary>
		/// <value>The location of the object.</value>
		public Coordinate Location { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.StaticObject"/> class.
		/// </summary>
		public StaticObject (Map parentMap)
		{
			this.parentMap = parentMap;
		}
	}
}

