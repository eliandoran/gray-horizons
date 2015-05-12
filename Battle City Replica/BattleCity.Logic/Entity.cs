using System;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents a moving object on the in-game map.
	/// </summary>
	public class Entity: IDisposable
	{
		Map parentMap;

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
		/// The location of this entity on the game map.
		/// </summary>
		public Coordinate Location;

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Entity"/> class.
		/// </summary>
		public Entity (Map parentMap)
		{
			this.parentMap = parentMap;
		}

		#region IDisposable implementation

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		/// <filterpriority>2</filterpriority>
		/// <remarks>Call <see cref="Dispose"/> when you are finished using the <see cref="BattleCity.Logic.Entity"/>. The
		/// <see cref="Dispose"/> method leaves the <see cref="BattleCity.Logic.Entity"/> in an unusable state. After calling
		/// <see cref="Dispose"/>, you must release all references to the <see cref="BattleCity.Logic.Entity"/> so the garbage
		/// collector can reclaim the memory that the <see cref="BattleCity.Logic.Entity"/> was occupying.</remarks>
		public void Dispose ()
		{
			ParentMap.QueueEntityRemoval (this);
		}

		#endregion
	}
}

