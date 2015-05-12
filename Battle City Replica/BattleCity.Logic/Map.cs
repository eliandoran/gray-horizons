using System;
using System.Collections.Generic;

namespace BattleCity.Logic
{
	/// <summary>
	/// Represents a game map containing tanks, walls, power-ups, etc.
	/// </summary>
	public class Map
	{
		Size mapSize;
		StaticObject[,] staticObjects;
		readonly List<Entity> entities = new List<Entity> (10);
		readonly List<Entity> entityRemovalList = new List<Entity> (0);

		/// <summary>
		/// Gets the size of the map.
		/// </summary>
		/// <value>The size of the map.</value>
		public Size MapSize {
			get {
				return mapSize;
			}
		}

		/// <summary>
		/// Gets a matrix containing static objects on the map.
		/// </summary>
		/// <value>The list of static objects.</value>
		public StaticObject[,] StaticObjects {
			get {
				return staticObjects;
			}
		}

		/// <summary>
		/// Gets a list containing the entities on this map.
		/// </summary>
		/// <value>The entities.</value>
		public List<Entity> Entities {
			get {
				return entities;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BattleCity.Logic.Map"/> class.
		/// </summary>
		public Map (Size mapSize)
		{
			this.mapSize	= mapSize;
			staticObjects 	= new StaticObject[mapSize.Width, mapSize.Height];
		}

		/// <summary>
		/// Adds the specified entity to this map.
		/// </summary>
		/// <param name="entity">The entity to add to this map.</param>
		public void AddEntity(Entity entity)
		{
			entities.Add (entity);
		}

		/// <summary>
		/// Removes the specified entity from this map.
		/// </summary>
		/// <param name="entity">The entity to remove from this map.</param>
		public void RemoveEntity(Entity entity)
		{
			entities.Remove (entity);
		}

		/// <summary>
		/// Gets the entity placed at the specified location.
		/// </summary>
		/// <returns>The <see cref="BattleCity.Logic.Entity"/> at the specified location, null if it does not exist.</returns>
		/// <param name="location">Location.</param>
		public Entity GetEntityAt(Coordinate location)
		{
			foreach (Entity entity in Entities)
				if (entity.Location.Equals (location))
					return entity;

			return null;
		}

		/// <summary>
		/// Adds the specified entity to the removal queue.
		/// </summary>
		/// <param name="entity">Entity.</param>
		public void QueueEntityRemoval(Entity entity)
		{
			entityRemovalList.Add (entity);
		}

		/// <summary>
		/// Removes the entities queued for removal.
		/// </summary>
		public void RemoveQueuedEntities()
		{
			foreach (Entity entity in entityRemovalList)
				entities.Remove (entity);

			entityRemovalList.Clear ();
		}

		/// <summary>
		/// Updates all child entities belonging to this map.
		/// </summary>
		public void Update()
		{
			foreach (Entity entity in entities)
				if (entity.GetType () == typeof(Projectile))
					((Projectile)entity).Update ();
		}
	}
}

