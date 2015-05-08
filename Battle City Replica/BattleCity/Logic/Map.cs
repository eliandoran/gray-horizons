using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using BattleCity.Logic;
using BattleCity.ThirdParty;
using BattleCity.Entities;
using BattleCity.StaticObjects;
using System.Collections.ObjectModel;
using System.Diagnostics;
using BattleCity.Extensions;

namespace BattleCity.Logic
{
    /// <summary>
    /// Represents a game map containing tanks, walls, power-ups, etc.
    /// </summary>
    [XmlInclude (typeof(Tank))]
    [XmlInclude (typeof(Projectile))]
    [XmlInclude (typeof(Wall))]
    public class Map
    {
        readonly List<ObjectBase> objectRemovalList = new List<ObjectBase> (0);
        readonly List<ObjectBase> objectAdditionList = new List<ObjectBase> (0);
        readonly Collection<StaticObject> objects = new Collection<StaticObject> ();
        readonly Collection<Entity> entities = new Collection<Entity> ();

        /// <summary>
        /// Gets the size of the map.
        /// </summary>
        /// <value>The size of the map.</value>
        [XmlElement ("Size")]
        public Vector2 MapSize { get; set; }

        /// <summary>
        /// Gets a matrix containing static objects on the map.
        /// </summary>
        /// <value>The list of static objects.</value>
        [XmlElement ("StaticObjects")]
        public Collection<StaticObject> Objects
        {
            get
            {
                return objects;
            }
        }

        /// <summary>
        /// Gets a list containing the entities on this map.
        /// </summary>
        /// <value>The entities.</value>
        public Collection<Entity> Entities
        {
            get
            {
                return entities;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.Map"/> class.
        /// </summary>
        public Map (Vector2 mapSize)
        {
            MapSize = mapSize;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.Map"/> class with the default size.
        /// </summary>
        public Map () : this (new Vector2 (13, 13))
        {

        }

        /// <summary>
        /// Add the specified object to the map.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        public void Add(ObjectBase obj)
        {
            var entity = obj as Entity;

            if (entity != null)
            {
                entity.ParentMap = this;
                Entities.Add (entity);
            }
            else
            {
                var staticObject = obj as StaticObject;

                if (staticObject != null)
                {
                    staticObject.ParentMap = this;
                    Objects.Add (staticObject);
                }
            }
        }

        /// <summary>
        /// Removes the specified object from the map.
        /// </summary>
        /// <param name="obj">The object to be removed.</param>
        public void Remove(ObjectBase obj)
        {
            var entity = obj as Entity;
            if (entity != null)
                Entities.Remove (entity);
            else
            {
                var staticObject = obj as StaticObject;

                if (staticObject != null)
                    Objects.Remove (staticObject);
            }
        }

        /// <summary>
        /// Adds the specified object to the map's removal queue.
        /// </summary>
        /// <param name="obj">The object to be removed.</param>
        public void QueueRemoval(ObjectBase obj)
        {
            objectRemovalList.Add (obj);
        }

        /// <summary>
        /// Adds the specified object to the map's addition queue.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        public void QueueAddition(ObjectBase obj)
        {
            obj.ParentMap = this;
            objectAdditionList.Add (obj);
        }

        /// <summary>
        /// Removes the objects queued for removal and adds the objects queued for addition.
        /// </summary>
        public void FlushQueues()
        {
            // Remove the objects queued for removal.
            foreach (ObjectBase obj in objectRemovalList)
                Remove (obj);
            objectRemovalList.Clear ();

            // Add the objects queued for addition.
            foreach (ObjectBase obj in objectAdditionList)
                Add (obj);
            objectAdditionList.Clear ();
        }

        /// <summary>
        /// Updates all child entities belonging to this map.
        /// </summary>
        public void Update(TimeSpan timePassed)
        {
            foreach (Entity entity in Entities)
                entity.Update (timePassed);

            FlushQueues ();

            foreach (StaticObject obj in Objects)
                obj.Update (timePassed);
        }

        public bool IntersectsMap(RotatedRectangle rect)
        {
            return rect.Intersects (new Rectangle (0, 0, (int)MapSize.X, (int)MapSize.Y));
        }

        public List<StaticObject> SearchStaticObject(RotatedRectangle rect)
        {
            var result = new List<StaticObject> ();

            foreach (StaticObject obj in Objects)
                if (obj.Position.Intersects (rect))
                {
                    #if DEBUG
                    Debug.WriteLine ("Found {0}.".FormatWith (obj.ToString ()), "SEARCH");
                    #endif

                    result.Add (obj);
                }

            return result;
        }

        /// <summary>
        /// Determines whether a rotated rectangle is within the bounds of this map.
        /// </summary>
        /// <returns><c>true</c> if the given rectangle is within bounds; otherwise, <c>false</c>.</returns>
        /// <param name="rect">The rotated rectangle.</param>
        public bool IsWithinBounds(RotatedRectangle rect)
        {
            //return (new RotatedRectangle (new Rect(MapSize.Width, MapSize.Height).ToRectangle(), 0).Intersects(rect));
            Vector2 upperLeft = rect.UpperLeftCorner ();
            Vector2 lowerRight = rect.LowerRightCorner ();

            if (rect.UpperLeftCorner ().X < 0 || rect.UpperLeftCorner ().Y < 0 ||
                rect.UpperRightCorner ().X < 0 || rect.UpperRightCorner ().Y < 0 ||
                rect.LowerLeftCorner ().X < 0 || rect.LowerLeftCorner ().Y < 0 ||
                rect.LowerRightCorner ().X < 0 || rect.LowerRightCorner ().Y < 0)
                return false;
				
            if (rect.UpperLeftCorner ().X > MapSize.X || rect.UpperLeftCorner ().Y > MapSize.Y ||
                rect.UpperRightCorner ().X > MapSize.X || rect.UpperRightCorner ().Y > MapSize.Y ||
                rect.LowerLeftCorner ().X > MapSize.X || rect.LowerLeftCorner ().Y > MapSize.Y ||
                rect.LowerRightCorner ().X > MapSize.X || rect.LowerRightCorner ().Y > MapSize.Y)
                return false;

            return true;
        }
    }
}

