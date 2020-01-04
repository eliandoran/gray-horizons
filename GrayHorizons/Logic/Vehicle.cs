namespace GrayHorizons.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using GrayHorizons.Entities;
    using GrayHorizons.Events;
    using GrayHorizons.Extensions;
    using GrayHorizons.StaticObjects;
    using Microsoft.Xna.Framework;

    public abstract class Vehicle: ControllableEntity
    {
        readonly List<Soldier> passengers = new List<Soldier>(0);

        public event EventHandler EngineStarting;

        public event EventHandler EngineStarted;

        public Point AxisPosition { get; set; }

        bool engineStarted, isEngineStarting;

        public bool UseDefaultEngineStartingSound { get; set; }

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
                else
                {
                    CurrentTimeToStart = TimeToStart;
                    IsEngineStarting = false;
                }
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
                {
                    CurrentTimeToStart = TimeSpan.FromMilliseconds(TimeToStart.TotalMilliseconds);
                    OnEngineStarting(EventArgs.Empty);
                }
            }
        }

        public bool CanBeBoarded { get; set; }

        bool canBeRunOverByTank;

        public TimeSpan TimeToStart { get; set; }

        public TimeSpan CurrentTimeToStart { get; set; }

        public TimeSpan IsMovingTime { get; set; }

        public TimeSpan CurrentIsMovingTime { get; set; }

        public bool CanBeRunOverByTank
        {
            get { return canBeRunOverByTank; }
            set
            {
                canBeRunOverByTank = value;

                if (canBeRunOverByTank)
                    Collided += Vehicle_Collided;
                else
                    Collided -= Vehicle_Collided;
            }
        }

        public List<Soldier> Passengers { get { return passengers; } }

        protected Vehicle()
        {
            Health = 1;
            AccelerationFactor = 0.05f;
            Acceleration = 0.05f;
            AxisPosition = new Point(DefaultSize.X / 2, DefaultSize.Y / 2);
            Moved += VehicleMoved;
            HasCollision = true;
            MiniMapColor = Color.Yellow;

            TimeToStart = TimeSpan.FromMilliseconds(2000);
            IsEngineStarted = false;
            UseDefaultEngineStartingSound = true;
            CanMoveOnSpot = false;
            CanBeRunOverByTank = false;

            IsMovingTime = TimeSpan.FromMilliseconds(100);
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
                    Debug.WriteLine("{0}'s engine started.".FormatWith(GetType()), "ENGINE");
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
            Debug.WriteLine(IsMoving);
            if (engineStarted && (IsMoving))
            {
                CurrentIsMovingTime = IsMovingTime;
                return base.Turn(turnDirection, moveDirection, noClip);
            }

            if (!IsEngineStarting)
            {
                Debug.WriteLine("Starting engine of {0}.".FormatWith(ToString()), "ENGINE");
                IsEngineStarting = true;
                return false;
            }

            return false;
        }

        public override bool Move(MoveDirection direction, bool noClip)
        {
            if (IsEngineStarted)
            {
                IsMoving = true;
                CurrentIsMovingTime = IsMovingTime;
                return base.Move(direction, noClip);
            }

            if (!IsEngineStarting)
            {
                Debug.WriteLine("Starting engine of {0}.".FormatWith(ToString()), "ENGINE");
                IsEngineStarting = true;
                return false;
            }

            IsMoving = false;
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
                new Vector2(
                    position.X + Position.CollisionRectangle.Width / 2,
                    position.Y + Position.CollisionRectangle.Height / 2),
                sourceRectangle: Renderer.GetSpriteFromSpriteImage(texture, 0, 2, 1),
                origin: new Vector2(
                    Position.CollisionRectangle.Width / 2,
                    Position.CollisionRectangle.Height / 2),
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
                passenger.Position = null;
                passenger.Location = new Point(
                    (int)(Position.X + Position.Width / 2),
                    Position.Y + Position.Height / 2
                );

                GameData.Map.CenterViewportAt(passenger);

                passenger.Moved += (sender, e) => GameData.Map.CenterViewportAt(passenger);
                GameData.ActivePlayer.AssignedEntity = passenger;

                HasCollision &= !Position.Intersects(passenger.Position);
            }

            // Turn the engine off if there are no passengers left in the vehicle.
            IsEngineStarted &= Passengers.Count != 0;
        }

        public override void Use()
        {
            if (Passengers.Count > 0)
                GetPassengerOff(Passengers[0], true);
        }

        public override void Destroy()
        {
            var wreck = new Wreck(GameData, this);
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

            if (CurrentIsMovingTime > gameTime)
                CurrentIsMovingTime -= gameTime;
            else
                IsMoving = false;

            base.Update(gameTime);
        }

        protected virtual void OnEngineStarted(EventArgs e)
        {
            if (EngineStarted.IsNotNull())
                EngineStarted(this, e);
        }

        protected virtual void OnEngineStarting(EventArgs e)
        {
            if (UseDefaultEngineStartingSound)
                Sound.VehicleSounds.Starting.Play();

            if (EngineStarting.IsNotNull())
                EngineStarting(this, e);
        }
    }
}

