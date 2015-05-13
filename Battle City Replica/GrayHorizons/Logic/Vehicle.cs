using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using GrayHorizons.Entities;
using System.Configuration;
using System.Collections.Generic;
using GrayHorizons.ThirdParty;

namespace GrayHorizons.Logic
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
            HasCollision = true;
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
            var position = GameData.Map.CalculateViewportCoordinates (new Vector2 (
                                   Position.CollisionRectangle.X,
                                   Position.CollisionRectangle.Y), 
                                                                      GameData.MapScale);            

            var scaledWidth = (int)(Position.Width * GameData.MapScale.X);
            var scaledHeight = (int)(Position.Height * GameData.MapScale.Y);

            var rect = new Rectangle (
                           (int)position.X,
                           (int)position.Y,
                           scaledWidth,
                           scaledHeight);

            var rrect = new RotatedRectangle (rect, Position.Rotation);

            GameData.SpriteBatch.Draw (
                texture,
                position: new Vector2 (Position.X, Position.Y),
                rotation: Position.Rotation,
                scale: GameData.MapScale
            );
        }

        public void BoardPassenger (
            Soldier passenger)
        {
            Passengers.Add (passenger);
            GameData.Map.QueueRemoval (passenger);
        }

        public void GetPassengerOff (
            Soldier passenger,
            bool passControl = false)
        {
            GameData.Map.QueueAddition (passenger);
            Passengers.Remove (passenger);

            if (GameData.ActivePlayer.AssignedEntity == this && passControl)
            {
                GameData.Map.CenterViewportAt (passenger);

                passenger.Moved += (
                    sender,
                    e) => GameData.Map.CenterViewportAt (passenger);

                GameData.ActivePlayer.AssignedEntity = passenger;
            }
        }

        public override void Use ()
        {
            if (Passengers.Count > 0)
            {
                GetPassengerOff (Passengers [0], true);
            }
        }
    }
}

