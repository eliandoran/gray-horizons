using System;
using System.Xml;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using BattleCity.ThirdParty;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using BattleCity.Extensions;
using System.Dynamic;
using System.Configuration;

namespace BattleCity.Logic
{
    public delegate void MovedEventHandler (object sender, EventArgs e);
    public delegate void UpdatedEventHandler (object sender, EventArgs e);

    /// <summary>
    /// Represents a moving object on the in-game map.
    /// </summary>
    public abstract class Entity: ObjectBase
    {
        public event MovedEventHandler Moved;
        public event UpdatedEventHandler Updated;

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

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.Entity"/> class.
        /// </summary>
        public Entity ()
        {

        }

        /// <summary>
        /// Determines whether this instance can move in a direction with the specified delta X and Y and step.
        /// </summary>
        /// <returns><c>true</c> if this instance can move in the specified direction; otherwise, <c>false</c>.</returns>
        /// <param name="deltaX">The horizontal offset.</param>
        /// <param name="deltaY">The vertical offset.</param>
        /// <param name="step">How much to move.</param>
        public bool CanMoveIn (
            RotatedRectangle newPosition)
        {
            if (!GameData.Map.IsWithinBounds (newPosition))
                return false;

            foreach (StaticObject obj in GameData.Map.StaticObjects)
            {
                if (obj.Position.Intersects (newPosition))
                {
                    #if DEBUG
                    Debug.WriteLine ("<{0}> ({1}) collides with <{2}> ({3})".FormatWith (ToString (),
                        Position,
                        obj,
                        obj.Position),
                        "COLLISION");
                    #endif

                    return false;
                }
            }

            foreach (Entity entity in GameData.Map.Entities)
            {
                if (entity != this && entity.Position.Intersects (newPosition) && entity.HasCollision)
                {
                    #if DEBUG
                    Debug.WriteLine ("<{0}> ({1}) collides with <{2}> ({3})".FormatWith (ToString (),
                        Position,
                        entity,
                        entity.Position),
                        "COLLISION");
                    #endif

                    return false;
                }
            }

            return true;
        }

        public static Point GetDelta (
            float rads,
            int step,
            bool reverse = false)
        {
            return new Point (
                (int)((reverse ? -1 : 1) * Math.Sin (rads) * step),
                (int)((reverse ? 1 : -1) * Math.Cos (rads) * step));
        }

        public virtual bool Move (
            MoveDirection direction,
            int step,
            bool noClip)
        {
            var rads = Rotation.FromRadians (Position.Rotation).OffsetBy (90).ToRadians ();
            var newPosition = new RotatedRectangle (Position.CollisionRectangle, Position.Rotation);
            var _step = (int)(Math.Round (AccelerationFactor * step));

            #if DEBUG
            Debug.WriteLine ("Factor: {0}, Step: {1}, FStep: {2}.".FormatWith (AccelerationFactor, step, _step),
                "MOVE");
            #endif

            var delta = GetDelta (rads, _step, (direction == MoveDirection.Backward));
            newPosition.ChangePosition (delta.X, delta.Y);

            if (noClip || CanMoveIn (newPosition))
            {
                Position = newPosition;

//                if (ParentMap.Viewport.X + ParentMap.Viewport.Width < newPosition.X)
//                {
//                    ParentMap.Viewport = new Rectangle (ParentMap.Viewport.X + 20,
//                                                        ParentMap.Viewport.Y,
//                                                        ParentMap.Viewport.Width,
//                                                        ParentMap.Viewport.Height);
//                    Debug.WriteLine ("Viewport shifted.");
//                }
//                else if (ParentMap.Viewport.X > newPosition.X)
//                {
//                    ParentMap.Viewport = new Rectangle (ParentMap.Viewport.X - 20,
//                                                        ParentMap.Viewport.Y,
//                                                        ParentMap.Viewport.Width,
//                                                        ParentMap.Viewport.Height);
//                }

                OnMoved (EventArgs.Empty);

                return true;
            }

            return false;
        }

        public virtual bool Turn (
            TurnDirection turnDirection,
            MoveDirection? moveDirection = null,
            bool onSpot = false,
            bool noClip = false)
        {
            const int step = 2;
            MoveDirection _moveDirection = moveDirection ?? MoveDirection.Backward;

            if (onSpot || Move (_moveDirection, 3, noClip))
            {
                var offset = (turnDirection == TurnDirection.Left ? -step : step);
                var newPosition = new RotatedRectangle (Position.CollisionRectangle, Position.Rotation);
                newPosition.Rotation = Rotation.FromRadians (newPosition.Rotation).OffsetBy (offset).ToRadians ();

                if (noClip || CanMoveIn (newPosition))
                {
                    Position = newPosition;
                    OnMoved (EventArgs.Empty);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// It should be called upon when the game requires this <see cref="BattleCity.Logic.Entity"/> to update.
        /// </summary>
        /// <param name="gameTime">The time that has elapsed since the last update.</param>
        public override void Update (
            TimeSpan gameTime)
        {
            base.Update (gameTime);
            OnUpdated (EventArgs.Empty);

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

        protected virtual void OnMoved (
            EventArgs e)
        {
            if (Moved != null)
                Moved (this, e);
        }

        protected virtual void OnUpdated (
            EventArgs e)
        {
            if (Updated != null)
                Updated (this, e);
        }

        public override void Render ()
        {
            Vector2 position;
            Texture2D texture;

            //if (entity.Position.CollisionRectangle.Intersects (gameData.Map.Viewport))
            if (true)
            {
                var entityType = GetType ();

                if (GameData.MappedTextures.ContainsKey (entityType))
                {
                    texture = GameData.MappedTextures [entityType];
                    position = GameData.Map.CalculateViewportCoordinates (Position.CollisionRectangle.Center.ToVector2 (),
                        GameData.Scale);
                }
                else
                {
                    #if DEBUG
                    Debug.WriteLine ("There is no mapped texture for type <{0}>.".FormatWith (entityType.Name),
                        "RENDERER");
                    #endif

                    return;
                }
            }
            else
            {
                Debug.WriteLine ("{0} - out of viewport.".FormatWith (ToString ()));
                return;
            }

            GameData.SpriteBatch.Draw (
                texture,
                position: position,
                    //destinationRectangle: new Rectangle (entity.RotatedPos.X, entity.RotatedPos.Y, entity.RotatedPos.Width, entity.RotatedPos.Height),
                origin: new Vector2 (texture.Width / 2, texture.Height / 2),
                rotation: Position.Rotation,
                scale: GameData.Scale
            );
        }
    }
}