using System;
using Microsoft.Xna.Framework;
using GrayHorizons.Entities;
using System.Collections.Generic;
using System.Diagnostics;
using GrayHorizons.StaticObjects;

namespace GrayHorizons.Logic
{
    public abstract class Vehicle: ControllableEntity
    {
        readonly List<Soldier> passengers = new List<Soldier>(0);

        public event EventHandler EngineStarted;

        public Point AxisPosition { get; set; }

        bool engineStarted, isEngineStarting;

        public bool IsEngineStarted
        {
            get
            {
                return engineStarted;
            }
            set
            {
                engineStarted = value;

                if (value)
                    OnEngineStarted(EventArgs.Empty);
            }
        }

        public bool IsEngineStarting
        {
            get
            {
                return isEngineStarting;
            }
            set
            {
                isEngineStarting = value;

                if (isEngineStarting)
                    CurrentTimeToStart = TimeSpan.FromMilliseconds(TimeToStart.TotalMilliseconds);
            }
        }

        public bool CanBeBoarded { get; set; }

        bool canBeRunOverByTank;

        public TimeSpan TimeToStart { get; set; }

        public TimeSpan CurrentTimeToStart { get; set; }

        public bool CanBeRunOverByTank
        {
            get
            {
                return canBeRunOverByTank;
            }
            set
            {
                canBeRunOverByTank = value;

                if (canBeRunOverByTank)
                {
                    Collided += Vehicle_Collided;
                }
                else
                {
                    Collided -= Vehicle_Collided;
                }
            }
        }

        public List<Soldier> Passengers
        {
            get
            {
                return passengers;
            }
        }

        protected Vehicle()
        {
            Health = 1;
            AccelerationFactor = 0.05f;
            Acceleration = 0.05f;
            AxisPosition = new Point(DefaultSize.X / 2, DefaultSize.Y / 2);
            Moved += VehicleMoved;
            HasCollision = true;
            MinimapColor = Color.Yellow;

            TimeToStart = TimeSpan.FromMilliseconds(500);
            IsEngineStarted = false;
        }

        void Vehicle_Collided(object sender, CollideEventArgs e)
        {
            if (e.CollidedWith is Tank)
            {
                HasCollision = false;
                e.PassThrough = true;
                Explode();
            }
        }

        void VehicleMoved(
            object sender,
            EventArgs e)
        {
            // TODO: Do something here.
        }

        public override bool Move(MoveDirection direction, bool noClip)
        {
            if (engineStarted)
            {
                IsMoving = true;
                return base.Move(direction, noClip);
            }

            if (!IsEngineStarting)
                IsEngineStarting = true;

            IsMoving = false;
            return false;
        }

        bool CheckEngine(TimeSpan gameTime)
        {
            if (IsEngineStarted)
                return true;

            if (!isEngineStarting)
                return false;

            if (!IsEngineStarted)
            {
                if (CurrentTimeToStart > TimeSpan.Zero)
                {
                    CurrentTimeToStart -= gameTime;
                    return false;
                }
                else
                {
                    Debug.WriteLine("{0}'s engine started.", GetType());
                    IsEngineStarted = true;
                    return true;
                }
            }
            else
                return true;
        }

        public override bool Turn(
            Entity.TurnDirection turnDirection,
            Entity.MoveDirection? moveDirection = default(Entity.MoveDirection?),
            bool noClip = false)
        {
            if (!engineStarted)
            {
                IsEngineStarting = true;
                return false;
            }

            if (IsMoving || CanMoveOnSpot)
                return base.Turn(turnDirection, moveDirection, noClip);

            return false;
        }

        public override void Render()
        {
            var texture = GameData.MappedTextures[GetType()];
            var position = GameData.Map.CalculateViewportCoordinates(new Vector2(
                                   Position.CollisionRectangle.X,
                                   Position.CollisionRectangle.Y), 
                               GameData.MapScale);            

            GameData.ScreenManager.SpriteBatch.Draw(
                texture,
                position: new Vector2(position.X + Position.CollisionRectangle.Width / 2,
                    position.Y + Position.CollisionRectangle.Height / 2),
                origin: new Vector2(Position.CollisionRectangle.Width / 2, Position.CollisionRectangle.Height / 2),
                rotation: Position.Rotation,
                scale: GameData.MapScale
            );
        }

        public void BoardPassenger(
            Soldier passenger)
        {
            Passengers.Add(passenger);
            GameData.Map.QueueRemoval(passenger);
        }

        public void GetPassengerOff(
            Soldier passenger,
            bool passControl = false)
        {
            GameData.Map.QueueAddition(passenger);
            Passengers.Remove(passenger);

            if (GameData.ActivePlayer.AssignedEntity == this && passControl)
            {
                GameData.Map.CenterViewportAt(passenger);

                passenger.Moved += (
                    sender,
                    e) => GameData.Map.CenterViewportAt(passenger);

                GameData.ActivePlayer.AssignedEntity = passenger;
            }
        }

        public override void Use()
        {
            if (Passengers.Count > 0)
            {
                GetPassengerOff(Passengers[0], true);
            }
        }

        public override void Destroy()
        {
            var wreck = new Wreck(GameData, this);
            wreck.HasCollision = false;
            GameData.Map.QueueAddition(wreck);

            base.Destroy();
        }

        public override void Shoot()
        {
            // Do nothing.
        }

        public override void Update(TimeSpan gameTime)
        {
            CheckEngine(gameTime);

            base.Update(gameTime);
        }

        protected virtual void OnEngineStarted(EventArgs e)
        {
            if (EngineStarted != null)
            {
                EngineStarted(this, e);
            }
        }
    }
}

