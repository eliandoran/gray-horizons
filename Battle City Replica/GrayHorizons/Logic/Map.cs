namespace GrayHorizons.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Xml.Serialization;
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.StaticObjects;
    using GrayHorizons.ThirdParty;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents a game map containing tanks, walls, power-ups, etc.
    /// </summary>
    [XmlInclude(typeof(Tank))]
    [XmlInclude(typeof(Projectile))]
    [XmlInclude(typeof(Wall))]
    public class Map: IRenderable
    {
        readonly List<ObjectBase> objectRemovalList = new List<ObjectBase>(0);
        readonly List<ObjectBase> objectAdditionList = new List<ObjectBase>(0);
        readonly Collection<StaticObject> staticObjects = new Collection<StaticObject>();
        readonly Collection<Entity> entities = new Collection<Entity>();
        protected internal Collection<CollisionBoundary> collisionBoundaries = new Collection<CollisionBoundary>();

        /// <summary>
        /// Gets the size of the map.
        /// </summary>
        /// <value>The size of the map.</value>
        [XmlElement("Size")]
        public Vector2 MapSize { get; set; }

        public Rectangle Viewport { get; set; }

        public Rectangle ScaledViewport { get; set; }

        public Texture2D Texture { get; set; }

        public GameData GameData { get; set; }

        public int ShakeFactor { get; set; }

        /// <summary>
        /// Gets a matrix containing static objects on the map.
        /// </summary>
        /// <value>The list of static objects.</value>
        public Collection<StaticObject> StaticObjects
        {
            get
            {
                return staticObjects;
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

        public Collection<CollisionBoundary> CollisionBoundaries
        {
            get
            {
                return collisionBoundaries;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.Map"/> class.
        /// </summary>
        public Map(
            Vector2 mapSize,
            GameData gameData)
        {
            MapSize = mapSize;
            GameData = gameData;

            if (gameData.IsNotNull())
            {
                gameData.ResolutionChanged += GameData_ResolutionChanged;
            }
        }

        void GameData_ResolutionChanged(object sender, EventArgs e)
        {
            Viewport = new Rectangle(0, 0, GameData.Game.GraphicsDevice.Viewport.Width, GameData.Game.GraphicsDevice.Viewport.Height);
            ScaledViewport = Viewport.ScaleTo(GameData.ViewportScale);

            if (GameData.ActivePlayer.IsNotNull() && GameData.ActivePlayer.AssignedEntity.IsNotNull())
                CenterViewportAt(GameData.ActivePlayer.AssignedEntity);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.Map"/> class with the default size.
        /// </summary>
        public Map()
            : this(
                new Vector2(13, 13),
                null)
        {

        }

        /// <summary>
        /// Add the specified object to the map.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        public void Add(
            ObjectBase obj)
        {
            var entity = obj as Entity;

            if (entity.IsNotNull())
            {
                entity.GameData = GameData;

                if (entity.AI.IsNotNull())
                {
                    entity.AI.GameData = GameData;
                }

                Entities.Add(entity);
            }
            else
            {
                var staticObject = obj as StaticObject;

                if (staticObject.IsNotNull())
                {
                    staticObject.GameData = GameData;
                    StaticObjects.Add(staticObject);
                }
            }
        }

        /// <summary>
        /// Removes the specified object from the map.
        /// </summary>
        /// <param name="obj">The object to be removed.</param>
        public void Remove(
            ObjectBase obj)
        {
            var entity = obj as Entity;
            if (entity.IsNotNull())
                Entities.Remove(entity);
            else
            {
                var staticObject = obj as StaticObject;

                if (staticObject.IsNotNull())
                    StaticObjects.Remove(staticObject);
            }
        }

        /// <summary>
        /// Adds the specified object to the map's removal queue.
        /// </summary>
        /// <param name="obj">The object to be removed.</param>
        public void QueueRemoval(
            ObjectBase obj)
        {
            objectRemovalList.Add(obj);
        }

        /// <summary>
        /// Adds the specified object to the map's addition queue.
        /// </summary>
        /// <param name="obj">The object to be added.</param>
        public void QueueAddition(
            ObjectBase obj)
        {
            obj.GameData = GameData;
            objectAdditionList.Add(obj);
        }

        /// <summary>
        /// Removes the objects queued for removal and adds the objects queued for addition.
        /// </summary>
        public void FlushQueues()
        {
            // Remove the objects queued for removal.
            objectRemovalList.ForEach(Remove);
            objectRemovalList.Clear();

            // Add the objects queued for addition.
            objectAdditionList.ForEach(Add);
            objectAdditionList.Clear();
        }

        /// <summary>
        /// Updates all child entities belonging to this map.
        /// </summary>
        public void Update(
            TimeSpan timePassed)
        {
            foreach (Entity entity in Entities)
                entity.Update(timePassed);

            FlushQueues();

            foreach (StaticObject obj in StaticObjects)
                obj.Update(timePassed);
        }

        public bool IntersectsMap(RotatedRectangle rect)
        {
            return rect.Intersects(new Rectangle(0, 0, (int)MapSize.X, (int)MapSize.Y));
        }

        public bool IntersectsViewport(RotatedRectangle rect)
        {
            return rect.Intersects(ScaledViewport);
        }

        public List<ObjectBase> SearchMapObjects(
            RotatedRectangle rect)
        {
            var result = new List<ObjectBase>();

            foreach (StaticObject obj in StaticObjects)
                if (obj.Position.Intersects(rect))
                {
                    result.Add(obj);
                }

            foreach (Entity obj in Entities)
                if (obj.Position.Intersects(rect))
                {
                    result.Add(obj);
                }

            return result;
        }

        /// <summary>
        /// Determines whether a rotated rectangle is within the bounds of this map.
        /// </summary>
        /// <returns><c>true</c> if the given rectangle is within bounds; otherwise, <c>false</c>.</returns>
        /// <param name="rect">The rotated rectangle.</param>
        public bool IsWithinBounds(
            RotatedRectangle rect)
        {
            Vector2 upperLeft = rect.UpperLeftCorner();
            Vector2 lowerRight = rect.LowerRightCorner();

            if (rect.UpperLeftCorner().X < 0 || rect.UpperLeftCorner().Y < 0 ||
                rect.UpperRightCorner().X < 0 || rect.UpperRightCorner().Y < 0 ||
                rect.LowerLeftCorner().X < 0 || rect.LowerLeftCorner().Y < 0 ||
                rect.LowerRightCorner().X < 0 || rect.LowerRightCorner().Y < 0)
                return false;
				
            if (rect.UpperLeftCorner().X > MapSize.X || rect.UpperLeftCorner().Y > MapSize.Y ||
                rect.UpperRightCorner().X > MapSize.X || rect.UpperRightCorner().Y > MapSize.Y ||
                rect.LowerLeftCorner().X > MapSize.X || rect.LowerLeftCorner().Y > MapSize.Y ||
                rect.LowerRightCorner().X > MapSize.X || rect.LowerRightCorner().Y > MapSize.Y)
                return false;

            return true;
        }

        public void CenterViewportAt(
            ObjectBase obj)
        {
            var pos = obj.Position.CollisionRectangle.Center;
            var x = pos.X - (ScaledViewport.Size.X / 2 + obj.Position.Width / 2);
            var y = pos.Y - (ScaledViewport.Size.Y / 2 + obj.Position.Height / 2);

            if (x < 0)
                x = 0;
            if (y < 0)
                y = 0;

            var right = x + ScaledViewport.Size.X;
            if (right > MapSize.X)
                x -= (int)(right - MapSize.X);

            var bottom = y + ScaledViewport.Size.Y;
            if (bottom > MapSize.Y)
                y -= (int)(bottom - MapSize.Y);

            ScaledViewport = new Rectangle(
                x, y,
                ScaledViewport.Width, ScaledViewport.Height
            );
        }

        public List<ObjectBase> GetObjects()
        {
            var result = new List<ObjectBase>();

            foreach (StaticObject obj in StaticObjects)
                result.Add(obj);
            foreach (Entity entity in Entities)
                result.Add(entity);

            return result;
        }

        public Vector2 CalculateViewportCoordinates(
            Vector2 realPosition,
            Vector2 scale)
        {
            return new Vector2(
                scale.X * (realPosition.X - ScaledViewport.X),
                scale.Y * (realPosition.Y - ScaledViewport.Y)
            );
        }

        public float CalculateDistance(
            ObjectBase first,
            ObjectBase second)
        {
            return (float)Math.Sqrt(
                Math.Pow(second.Position.X - first.Position.X, 2) +
                Math.Pow(second.Position.Y - first.Position.Y, 2));
        }

        Random random = new Random();

        Vector3 Shake()
        {
            return new Vector3(
                (float)((random.NextDouble() * 2) - 1),
                (float)((random.NextDouble() * 2) - 1), 0);
        }

        #region IRenderable implementation

        public void Render()
        {
            var mapSize = MapSize;

            GameData.TranslationMatrix = Matrix.CreateTranslation(ShakeFactor * Shake());

            Texture2D texture;
            if (GameData.MappedTextures.ContainsKey(GetType()))
                texture = GameData.MappedTextures[GetType()];
            else
            {
                var attr = new MappedTexturesAttribute(new [] { "Dirt/01" });
                texture = GameData.ContentManager.Load<Texture2D>(attr.GetRandomTexture());
            }

            Texture = texture;

            if (texture.Width >= mapSize.X && texture.Height >= mapSize.Y)
            {
                var pos = GameData.Map.CalculateViewportCoordinates(
                              Vector2.Zero,
                              GameData.MapScale);

                GameData.ScreenManager.SpriteBatch.Draw(
                    texture,
                    pos,
                    scale: GameData.MapScale);
            }
            else
            {
                int rows = (int)(Math.Ceiling(mapSize.Y / texture.Height));
                int cols = (int)(Math.Ceiling(mapSize.X / texture.Width));

                for (var row = 0; row <= rows; row++)
                    for (var col = 0; col <= cols; col++)
                    {
                        var rect = new Rectangle(
                                       col * texture.Width,
                                       row * texture.Height,
                                       texture.Width,
                                       texture.Height
                                   );

                        var pos = GameData.Map.CalculateViewportCoordinates(
                                      new Vector2(
                                          rect.X,
                                          rect.Y
                                      ),
                                      GameData.MapScale);

                        GameData.ScreenManager.SpriteBatch.Draw(
                            texture,
                            pos,
                            scale: GameData.MapScale
                        );
                    }
            }
        }

        #endregion
    }
}

