namespace GrayHorizons.Logic
{
    using System;
    using System.Diagnostics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using GrayHorizons.Events;
    using GrayHorizons.Extensions;
    using GrayHorizons.ThirdParty;

    /// <summary>
    /// Represents a moving object on the in-game map.
    /// </summary>
    public abstract class Entity: ObjectBase
    {
        AIBase ai;

        public event EventHandler Moved;
        public event EventHandler Updated;

        public enum TurnDirection
        {
            Left,
            Right
        }


        public enum MoveDirection
        {
            Forward,
            Backward
        }

        public float Acceleration { get; set; }

        public float AccelerationFactor { get; set; }

        public bool IsMoving { get; set; }

        public bool IsTurning { get; set; }

        public int Speed { get; set; }

        public bool CanMoveOnSpot { get; set; }

        /// <summary>
        /// Gets or sets artifical intelligence/player instance that controls this tank.
        /// </summary>
        /// <value>The AI/player controlling this instance.</value>
        public AIBase AI
        {
            get
            {
                return ai;
            }
            set
            {
                ai = value;
                ai.ControllingEntity = this;
                ai.GameData = GameData;
            }
        }

        /// <summary>
        /// Determines whether this instance can move in a direction with the specified delta X and Y and step.
        /// </summary>
        /// <returns><c>true</c> if this instance can move in the specified direction; otherwise, <c>false</c>.</returns>
        /// <param name="deltaX">The horizontal offset.</param>
        /// <param name="deltaY">The vertical offset.</param>
        /// <param name="step">How much to move.</param>
        public bool CanMoveIn(
            RotatedRectangle newPosition)
        {
            if (!GameData.Map.IsWithinBounds(newPosition))
            {
                Debug.WriteLine("Out of bounds.", "MOVE");

                return false;
            }

            foreach (ObjectBase obj in GameData.Map.GetObjects())
            {
                if (obj.HasCollision && obj.Position.Intersects(newPosition) && obj != this)
                {
                    Debug.WriteLine("<{0}> ({1}) collides with <{2}> ({3})".FormatWith(ToString(),
                            Position,
                            obj,
                            obj.Position),
                        "COLLISION");

                    var eventArgs = new CollideEventArgs(this);
                    obj.OnCollide(eventArgs);
                    return eventArgs.PassThrough;
                }
            }

            foreach (CollisionBoundary boundary in GameData.Map.CollisionBoundaries)
            {
                if (boundary.ToRotatedRectangle().Intersects(newPosition))
                {
                    Debug.WriteLine("<{0}> collides with a boundary.".FormatWith(ToString()));
                    return false;
                }
            }

            return true;
        }

        public static Point GetDelta(
            float radians,
            int step,
            bool reverse = false)
        {
            return new Point(
                (int)((reverse ? -1 : 1) * Math.Sin(radians) * step),
                (int)((reverse ? 1 : -1) * Math.Cos(radians) * step));
        }

        public virtual bool Move(
            MoveDirection direction,
            bool noClip)
        {
            var rads = Rotation.FromRadians(Position.Rotation).OffsetBy(90).ToRadians();
            var newPosition = new RotatedRectangle(Position.CollisionRectangle, Position.Rotation);
            var _step = (int)(Math.Round(AccelerationFactor * Speed));

            Debug.WriteLine("Factor: {0}, Speed: {1}, FStep: {2}.".FormatWith(AccelerationFactor, Speed, _step),
                "MOVE");

            var delta = GetDelta(rads, _step, (direction == MoveDirection.Backward));
            newPosition.ChangePosition(delta.X, delta.Y);

            if (noClip || CanMoveIn(newPosition))
            {
                Position = newPosition;
                OnMoved(EventArgs.Empty);

                return true;
            }

            return false;
        }

        public virtual bool Turn(
            TurnDirection turnDirection,
            MoveDirection? moveDirection = null,
            bool noClip = false)
        {
            const int step = 2;
            MoveDirection _moveDirection = moveDirection ?? MoveDirection.Backward;

            if (CanMoveOnSpot || Move(_moveDirection, noClip))
            {
                var offset = (turnDirection == TurnDirection.Left ? -step : step);
                var newPosition = new RotatedRectangle(Position.CollisionRectangle, Position.Rotation);
                newPosition.Rotation = Rotation.FromRadians(newPosition.Rotation).OffsetBy(offset).ToRadians();

                if (noClip || CanMoveIn(newPosition))
                {
                    Position = newPosition;
                    OnMoved(EventArgs.Empty);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// It should be called upon when the game requires this <see cref="GrayHorizons.Logic.Entity"/> to update.
        /// </summary>
        /// <param name="gameTime">The time that has elapsed since the last update.</param>
        public override void Update(
            TimeSpan gameTime)
        {
            base.Update(gameTime);
            OnUpdated(EventArgs.Empty);

            if (IsMoving)
            {
                if (AccelerationFactor < 1f)
                    AccelerationFactor += Acceleration;
            }
            else
            {
                AccelerationFactor = 0;
            }                
        }

        protected virtual void OnMoved(
            EventArgs e)
        {
            if (Moved.IsNotNull())
                Moved(this, e);
        }

        protected virtual void OnUpdated(
            EventArgs e)
        {
            if (Updated.IsNotNull())
                Updated(this, e);
        }

        public override void Render()
        {
            Vector2 position;
            Texture2D texture;

            //if (entity.Position.CollisionRectangle.Intersects (gameData.Map.Viewport))
            if (true)
            {
                var entityType = GetType();

                if (GameData.MappedTextures.ContainsKey(entityType))
                {
                    texture = GameData.MappedTextures[entityType];

                    position = GameData.Map.CalculateViewportCoordinates(Position.CollisionRectangle.Center.ToVector2(),
                        GameData.MapScale);
                }
                else
                {
                    Debug.WriteLine("There is no mapped texture for type <{0}>.".FormatWith(entityType.Name),
                        "RENDERER");

                    return;
                }
            }
            else
            {
                //Debug.WriteLine("{0} - out of viewport.".FormatWith(ToString()));
                //return;
            }

            GameData.ScreenManager.SpriteBatch.Draw(
                texture,
                destinationRectangle: new Rectangle((int)position.X, (int)position.Y, DefaultSize.X, DefaultSize.Y),
                origin: new Vector2(texture.Width / 2, texture.Height / 2),
                rotation: Position.Rotation,
                scale: GameData.MapScale
            );
        }

        public override void RenderHud()
        {
            // No implementation needed.
        }

        public abstract void Shoot();
    }
}