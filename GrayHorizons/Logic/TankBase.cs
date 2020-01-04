namespace GrayHorizons.Logic
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Xml.Serialization;
    using GrayHorizons.Entities.Projectiles;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using GrayHorizons.StaticObjects;
    using GrayHorizons.ThirdParty;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an in-game tank entity.
    /// </summary>
    public class Tank: Vehicle
    {
        bool hasMoved, hasFirstMoved, fadeOut;

        TimeSpan hasMovedTime;

        AudioEmitter emitter = new AudioEmitter();
        AudioListener listener = new AudioListener();

        SoundEffectInstance
            tankIdleSoundInstance,
            tankMovingSoundInstance;

        TimeSpan
            tankIdleVolumeFadeDuration,
            tankMovingVolumeFadeDuration;

        /// <summary>
        /// Gets or sets the score gained by a player if he destroys the tank.
        /// </summary>
        /// <value>The tank's score.</value>
        [XmlElement("Score"), DefaultValue(0)]
        public int Score { get; set; }

        /// <summary>
        /// Configures a projectile to be fired at the tank's behest.
        /// </summary>
        public override void Shoot()
        {
            var projectile = new TankShell();

            if (CoolDown > TimeSpan.Zero)
            {
                Debug.WriteLine("<{0}> is still under cooldown.".FormatWith(ToString()), "SHOOT");

                return;
            }

            if (AmmoLeft < 1)
            {
                Debug.WriteLine("SHOOT: {0} is out of ammunition.".FormatWith(ToString()), "SHOOT");
                Sound.TankSounds.NoAmmo.Play();
                return;
            }

            if (CoolDown == TimeSpan.Zero)
            {
                var xOffset = new Point(projectile.DefaultSize.X, 0);
                var yOffset = new Point(0, projectile.DefaultSize.Y);
                var muzzlePos = GetMuzzleRotatedRectangle();
                if (muzzlePos.IsNull())
                    return;

                var rect = muzzlePos.Offset(xOffset).Offset(yOffset);

                Debug.WriteLine("ROTATION: " + Rotation.FromRadians(TurretRect.Rotation));

                projectile.Position = rect;
                projectile.Owner = this;

                var explosion = new Explosion();
                explosion.Position = new RotatedRectangle(muzzlePos.CollisionRectangle, muzzlePos.Rotation);
                GameData.Map.QueueAddition(explosion);
                GameData.Map.QueueAddition(projectile);
                CoolDown = projectile.CoolTimePenalty;
                AmmoLeft--;

                Debug.WriteLine("<{0}> shot with <{1}>.".FormatWith(ToString(), projectile), "SHOOT");

                Sound.TankSounds.Firing.Play();

                return;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.Tank"/> class.
        /// </summary>
        public Tank()
        {
            HasCollision = true;
            IsInvincible = false;
            CanMoveOnSpot = true;

            Health = 5;
            Score = 0;
            AccelerationFactor = 0.025f;
            Acceleration = 0.05f;
            Speed = 6;
            AmmoLeft = 10;
            TurretRect = new RotatedRectangle();
            TurretRotation = new Rotation(0);


            DestructionDelay = TimeSpan.FromMilliseconds(500);
            MiniMapColor = Color.Red;
            UseDefaultEngineStartingSound = false;

            Moved += TankMoved;
            EngineStarting += Tank_EngineStarting;
        }

        void Tank_EngineStarting(object sender, EventArgs e)
        {
            Sound.TankSounds.Starting.Play();
        }

        void TankMoved(
            object sender,
            EventArgs e)
        {
            if (!hasMoved)
            {
                hasFirstMoved = true;
            }

            hasMoved = true;
            hasMovedTime = TimeSpan.FromMilliseconds(50);
            TurretRect.CollisionRectangle.X = Position.CollisionRectangle.X;
            TurretRect.CollisionRectangle.Y = Position.CollisionRectangle.Y;
        }

        /// <summary>
        /// Calls the underlying artificial intelligence or player to update the state of the tank.
        /// </summary>
        public override void Update(
            TimeSpan gameTime)
        {
            base.Update(gameTime);

            if (tankMovingSoundInstance.IsNull())
            {
                tankMovingSoundInstance = Sound.TankSounds.Moving.GetInstance();
                tankMovingSoundInstance.IsLooped = true;
            }
            if (tankIdleSoundInstance.IsNull())
            {
                tankIdleSoundInstance = Sound.TankSounds.Idle.GetInstance();
                tankIdleSoundInstance.IsLooped = true;
            }
            else
            {
                emitter.Position = new Vector3(Position.X, Position.Y, 0);

                listener.Position = new Vector3(GameData.ActivePlayer.AssignedEntity.Position.X,
                    GameData.ActivePlayer.AssignedEntity.Position.Y,
                    0);
            }

            if (CoolDown > gameTime)
                CoolDown = CoolDown.Subtract(gameTime);
            else
                CoolDown = TimeSpan.Zero;

            if (AI.IsNotNull())
                AI.NextStep();

//            Debug.WriteLine ("(hasMoved: {0}, IsMoving: {1}, IsTurning: {2}, hasFirstMoved: {3}".FormatWith (
//                hasMoved,
//                IsMoving,
//                IsTurning,
//                hasFirstMoved));


            var fadeInTime = TimeSpan.FromMilliseconds(1000);
            var fadeOutTime = TimeSpan.FromMilliseconds(500);

            if (hasMoved && (!IsMoving || !IsTurning))
            {
                if (hasMovedTime.TotalMilliseconds > 0)
                    hasMovedTime = hasMovedTime.Subtract(gameTime);
                else
                    hasMoved = false;

                if (hasMoved)
                {                
                    //Debug.WriteLine ("MOVE");
                    if (hasFirstMoved)
                    {                        
                        //tankIdleSoundInstance.Stop ();
                        fadeOut = false;
                        tankMovingVolumeFadeDuration = TimeSpan.FromMilliseconds(fadeInTime.TotalMilliseconds);
                        tankMovingSoundInstance.Volume = 0;
                        tankMovingSoundInstance.Play();

                        hasFirstMoved = false;
                    }
                }
                else
                {                
                    tankIdleSoundInstance.Play();
                    fadeOut = true;
                    tankMovingVolumeFadeDuration = TimeSpan.FromMilliseconds(fadeOutTime.TotalMilliseconds);
                
                    //Debug.WriteLine ("IDLE");
                }
            }

            if (tankMovingVolumeFadeDuration > gameTime)
            {
                tankMovingVolumeFadeDuration = tankMovingVolumeFadeDuration.Subtract(gameTime);

                if (!fadeOut)
                    SetVolumeFade(tankMovingSoundInstance, tankMovingVolumeFadeDuration, fadeInTime);
                else
                    SetVolumeFade(tankMovingSoundInstance, tankMovingVolumeFadeDuration, fadeOutTime, true);
            }
            else
            {
                if (fadeOut)
                    tankMovingSoundInstance.Stop();
            }
        }

        void SetVolumeFade(
            SoundEffectInstance fxInstance,
            TimeSpan fadeCurrentTime,
            TimeSpan fadeTotalTime,
            bool reverse = false)
        {
            
            var current = fadeCurrentTime.TotalMilliseconds;
            var total = fadeTotalTime.TotalMilliseconds;

            if (current > total)
                throw new ArgumentOutOfRangeException("fadeCurrentTime");

            var percentage = (float)(current / total);

            if (!reverse)
                percentage = 1 - percentage;
            
            fxInstance.Volume = percentage;
        }

        public override void Destroy()
        {            
            var wreck = new Wreck(GameData, this);
            GameData.Map.QueueAddition(wreck);

            if (tankIdleSoundInstance.IsNotNull())
                tankIdleSoundInstance.Stop();

            if (tankMovingSoundInstance.IsNotNull())
                tankMovingSoundInstance.Stop();

            base.Destroy();
        }

        public override void Explode()
        {
            Sound.ExplosionSounds.TankExplosion.Play();
            GenerateExplosion(Position.CollisionRectangle.Center, new Vector2(Position.CollisionRectangle.Width / 3));

            var random = new Random();
            for (int explosionNr = 0; explosionNr < 5; explosionNr++)
            {
                var x = Position.CollisionRectangle.X + random.Next(Position.CollisionRectangle.Width);
                var y = Position.CollisionRectangle.Y + random.Next(Position.CollisionRectangle.Height);

                var width = random.Next(16, (int)(Position.CollisionRectangle.Width / 1.5));
                var height = random.Next(16, (int)(Position.CollisionRectangle.Height / 1.5));

                Sound.ExplosionSounds.ExplosionEcho.Play();
                GenerateExplosion(new Point(x, y), new Vector2(width, height));
            }
        }

        public override void Render()
        {
            const int tankSpriteImageColumns = 1;
            const int tankSpriteImageRows = 2;
            const int tankIndex = 1;
            const int tankTurrel = 0;

            var texture = GameData.MappedTextures[GetType()];
            var position = GameData.Map.CalculateViewportCoordinates(Position.CollisionRectangle.Center.ToVector2(),
                               GameData.MapScale);

            var tankRect = Renderer.GetSpriteFromSpriteImage(texture,
                               tankIndex,
                               tankSpriteImageRows,
                               tankSpriteImageColumns);
            GameData.ScreenManager.SpriteBatch.Draw(
                texture,
                position,
                sourceRectangle: tankRect,
                origin: new Vector2(tankRect.Width / 2, tankRect.Height / 2),
                rotation: Position.Rotation,
                scale: GameData.MapScale
            );

            var turrelRect = Renderer.GetSpriteFromSpriteImage(texture,
                                 tankTurrel,
                                 tankSpriteImageRows,
                                 tankSpriteImageColumns);            
            GameData.ScreenManager.SpriteBatch.Draw(
                texture,
                position,
                sourceRectangle: turrelRect,
                origin: new Vector2(tankRect.Width / 2, tankRect.Height / 2),
                rotation: TurretRotation.ToRadians(),
                scale: GameData.MapScale
            );
        }

        protected override void OnEngineStarted(EventArgs e)
        {
            Debug.WriteLine("Engine started");

            if (tankIdleSoundInstance.IsNull())
            {
                tankIdleSoundInstance = Sound.TankSounds.Idle.GetInstance();
                tankIdleSoundInstance.IsLooped = true;
            }

            tankIdleSoundInstance.Play();
            base.OnEngineStarted(e);
        }

        public static List<Type> GetTankTypes()
        {
            return Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type => (type.BaseType == typeof(Tank)))
                .ToList();
        }
    }
}

