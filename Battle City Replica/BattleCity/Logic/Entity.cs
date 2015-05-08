using System;
using System.Xml;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using BattleCity.ThirdParty;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using BattleCity.Extensions;

namespace BattleCity.Logic
{
    /// <summary>
    /// Represents a moving object on the in-game map.
    /// </summary>
    public abstract class Entity: ObjectBase
    {
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
        public bool CanMoveIn(RotatedRectangle newPosition)
        {
            if (!ParentMap.IsWithinBounds (newPosition))
                return false;

            foreach (StaticObject obj in ParentMap.Objects)
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

            return true;
        }

        public static Point GetDelta(float rads,
                                     int step,
                                     bool reverse = false)
        {
            return new Point (
                (int)((reverse ? -1 : 1) * Math.Sin (rads) * step),
                (int)((reverse ? 1 : -1) * Math.Cos (rads) * step));
        }

        public virtual bool Move(MoveDirection direction,
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
                return true;
            }

            return false;
        }

        public virtual bool Turn(TurnDirection turnDirection,
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
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// It should be called upon when the game requires this <see cref="BattleCity.Logic.Entity"/> to update.
        /// </summary>
        /// <param name="gameTime">The time that has elapsed since the last update.</param>
        public override void Update(TimeSpan gameTime)
        {
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
    }
}