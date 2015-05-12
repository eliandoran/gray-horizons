using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using BattleCity.Entities;
using System.Configuration;
using System.Collections.Generic;

namespace BattleCity.Logic
{
    public abstract class Vehicle: ControllableEntity
    {
        readonly List<Soldier> passengers = new List<Soldier> (0);

        public Point AxisPosition { get; set; }

        public bool CanMoveOnSpot { get; set; }

        public bool EngineStarted { get; set; }

        public bool CanBeBoarded { get; set; }

        public List<Soldier> Passengers
        {
            get
            {
                return passengers;
            }
        }

        protected Vehicle ()
        {
            Health = 1;
            AccelerationFactor = 0.05f;
            Acceleration = 0.05f;
            AxisPosition = new Point (DefaultSize.X / 2, DefaultSize.Y / 2);
            Moved += VehicleMoved;
        }

        void VehicleMoved (
            object sender,
            EventArgs e)
        {
            // TODO: Do something here.
        }

        public override bool Turn (
            Entity.TurnDirection turnDirection,
            Entity.MoveDirection? moveDirection = default(Entity.MoveDirection?),
            bool onSpot = false,
            bool noClip = false)
        {
            if (IsMoving || CanMoveOnSpot)
                return base.Turn (turnDirection, moveDirection, onSpot, noClip);
            else
                return false;
        }

        public override void Render ()
        {
            var texture = GameData.MappedTextures [GetType ()];
            var position = GameData.Map.CalculateViewportCoordinates (Position.CollisionRectangle.Center.ToVector2 (),
                               GameData.Scale);

            GameData.SpriteBatch.Draw (
                texture,
                position: position,
                origin: new Vector2 (texture.Width / 2, texture.Height / 2),
                rotation: Position.Rotation,
                scale: GameData.Scale
            );
        }

        public void BoardPassenger (
            Soldier passenger)
        {
            Passengers.Add (passenger);
            GameData.Map.QueueRemoval (passenger);
        }

        public void UnboardPassenger (
            Soldier passenger)
        {
            GameData.Map.QueueAddition (passenger);
            Passengers.Remove (passenger);
        }

        public override void Use ()
        {
            if (Passengers.Count > 0)
            {
                UnboardPassenger (Passengers [0]);
            }
        }
    }
}

