using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;
using GrayHorizons.ThirdParty;
using GrayHorizons.Extensions;
using System.Collections;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GrayHorizons.Entities
{
    /// <summary>
    /// Represents an in-game tank entity.
    /// </summary>
    public class Tank: Vehicle
    {
        AIBase ai;

        bool hasMoved, hasFirstMoved, fadeOut;

        TimeSpan hasMovedTime;

        SoundEffectInstance tankIdleSoundInstance,
            tankMovingSoundInstance;

        TimeSpan tankIdleVolumeFadeDuration,
            tankMovingVolumeFadeDuration;

        /// <summary>
        /// Gets or sets the score gained by a player if he destroys the tank.
        /// </summary>
        /// <value>The tank's score.</value>
        [XmlElement ("Score"), DefaultValue (0)]
        public int Score { get; set; }

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
                ai.ControllingTank = this;
                ai.GameData = GameData;
            }
        }

        public RotatedRectangle MuzzlePosition { get; set; }

        public TimeSpan CoolDown { get; set; }

        /// <summary>
        /// Configures a projectile to be fired at the tank's behest.
        /// </summary>
        public virtual void Shoot (
            Projectile projectile)
        {
            if (CoolDown == TimeSpan.Zero)
            {
                var originX = (int)(Position.LowerLeftCorner ().X - Position.Origin.X);
                var originY = (int)(Position.LowerLeftCorner ().Y - Position.Origin.Y);

                var muzzlePos = GetMuzzleRotatedRectangle ();
                Debug.WriteLine ("ROTATION: " + Rotation.FromRadians (TurretRect.Rotation));
                var rect = new RotatedRectangle (
                               new Rectangle (
                                   (int)(muzzlePos.CollisionRectangle.Center.X),
                                   (int)(muzzlePos.CollisionRectangle.Center.Y),
                                   25,
                                   25), TurretRect.Rotation);

                projectile.Position = rect;
                projectile.OwnerTank = this;
                GameData.Map.QueueAddition (projectile);
                //Cooldown = projectile.CooltimePenalty;

                #if DEBUG
                Debug.WriteLine ("<{0}> shot with <{1}>.".FormatWith (ToString (), projectile), "SHOOT");
                #endif

                return;
            }

            #if DEBUG
            Debug.WriteLine ("<{0}> is still under cooldown.".FormatWith (ToString ()), "SHOOT");
            #endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Logic.Tank"/> class.
        /// </summary>
        public Tank ()
        {
            HasCollision = true;
            IsInvincible = false;
            CanMoveOnSpot = true;

            Health = 1;
            Score = 0;
            AccelerationFactor = 0.025f;
            Acceleration = 0.05f;
            TurretRect = new RotatedRectangle ();
            TurretRotation = new Rotation (0);

            DestructionDelay = TimeSpan.FromMilliseconds (500);

            Moved += TankMoved;
        }

        void TankMoved (
            object sender,
            EventArgs e)
        {
            if (!hasMoved)
            {
                hasFirstMoved = true;
            }
            hasMoved = true;
            hasMovedTime = TimeSpan.FromMilliseconds (50);
            TurretRect.CollisionRectangle.X = Position.CollisionRectangle.X;
            TurretRect.CollisionRectangle.Y = Position.CollisionRectangle.Y;
        }

        /// <summary>
        /// Calls the underlying artificial intelligence or player to update the state of the tank.
        /// </summary>
        public override void Update (
            TimeSpan gameTime)
        {
            base.Update (gameTime);

            if (tankMovingSoundInstance == null)
            {
                tankMovingSoundInstance = Sound.TankSounds.Moving.GetInstance ();
                tankMovingSoundInstance.IsLooped = true;
            }
            if (tankIdleSoundInstance == null)
            {
                tankIdleSoundInstance = Sound.TankSounds.Idle.GetInstance ();
                tankIdleSoundInstance.IsLooped = true;
            }

            if (CoolDown > gameTime)
                CoolDown = CoolDown.Subtract (gameTime);
            else
                CoolDown = TimeSpan.Zero;

            if (AI != null)
                AI.NextStep ();

//            Debug.WriteLine ("(hasMoved: {0}, IsMoving: {1}, IsTurning: {2}, hasFirstMoved: {3}".FormatWith (
//                hasMoved,
//                IsMoving,
//                IsTurning,
//                hasFirstMoved));


            var fadeInTime = TimeSpan.FromMilliseconds (1000);
            var fadeOutTime = TimeSpan.FromMilliseconds (500);

            if (hasMoved && (!IsMoving || !IsTurning))
            {
                if (hasMovedTime.TotalMilliseconds > 0)
                    hasMovedTime = hasMovedTime.Subtract (gameTime);
                else
                    hasMoved = false;

                if (hasMoved)
                {                
                    //Debug.WriteLine ("MOVE");
                    if (hasFirstMoved)
                    {                        
                        //tankIdleSoundInstance.Stop ();
                        fadeOut = false;
                        tankMovingVolumeFadeDuration = TimeSpan.FromMilliseconds (fadeInTime.TotalMilliseconds);
                        tankMovingSoundInstance.Volume = 0;
                        tankMovingSoundInstance.Play ();

                        hasFirstMoved = false;
                    }
                }
                else
                {                
                    //tankIdleSoundInstance.Play ();
                    fadeOut = true;
                    tankMovingVolumeFadeDuration = TimeSpan.FromMilliseconds (fadeOutTime.TotalMilliseconds);
                
                    //Debug.WriteLine ("IDLE");
                }
            }

            if (tankMovingVolumeFadeDuration > gameTime)
            {
                tankMovingVolumeFadeDuration = tankMovingVolumeFadeDuration.Subtract (gameTime);

                if (!fadeOut)
                    SetVolumeFade (tankMovingSoundInstance, tankMovingVolumeFadeDuration, fadeInTime);
                else
                    SetVolumeFade (tankMovingSoundInstance, tankMovingVolumeFadeDuration, fadeOutTime, true);
            }
            else
            {
                if (fadeOut)
                    tankMovingSoundInstance.Stop ();
            }
        }

        void SetVolumeFade (
            SoundEffectInstance fxInstance,
            TimeSpan fadeCurrentTime,
            TimeSpan fadeTotalTime,
            bool reverse = false)
        {
            
            var current = fadeCurrentTime.TotalMilliseconds;
            var total = fadeTotalTime.TotalMilliseconds;

            if (current > total)
                throw new ArgumentOutOfRangeException ("fadeCurrentTime");

            var percentage = (float)(current / total);

            if (!reverse)
                percentage = 1 - percentage;
            
            Debug.WriteLine (percentage);
            fxInstance.Volume = percentage;
        }

        public static RotatedRectangle OffsetRotatedRectangle (
            RotatedRectangle rect,
            Point offsetBy)
        {
            var newRect = new RotatedRectangle (rect.CollisionRectangle, rect.Rotation);
            var rads = rect.Rotation;

            var deltaX = new Point (
                             (int)(Math.Cos (rads) * offsetBy.X),
                             (int)(Math.Sin (rads) * offsetBy.X));
            var deltaY = new Point (
                             (int)(Math.Sin (rads) * -offsetBy.Y),
                             (int)(Math.Cos (rads) * offsetBy.Y)
                         );

            newRect.CollisionRectangle.Offset (deltaX);
            newRect.CollisionRectangle.Offset (deltaY);
            return newRect;
        }

        public RotatedRectangle GetMuzzleRotatedRectangle ()
        {    
            MuzzlePosition.Rotation = TurretRotation.ToRadians ();
            TurretRect.Rotation = TurretRotation.ToRadians ();
            var muzzleX = ((MuzzlePosition != null) ? MuzzlePosition.X : 0);
            var muzzleY = ((MuzzlePosition != null) ? MuzzlePosition.Y : 0);

            var rect = new Rectangle (
                           (int)(TurretRect.UpperLeftCorner ().X),
                           (int)(TurretRect.UpperLeftCorner ().Y),
                           ((MuzzlePosition != null) ? MuzzlePosition.CollisionRectangle.Width : 0),
                           ((MuzzlePosition != null) ? MuzzlePosition.CollisionRectangle.Height : 0)
                       );

            var rads = TurretRotation.ToRadians ();

            var deltaX = new Point (
                             (int)(Math.Cos (rads) * muzzleX),
                             (int)(Math.Sin (rads) * muzzleX));
            var deltaY = new Point (
                             (int)(Math.Sin (rads) * -muzzleY),
                             (int)(Math.Cos (rads) * muzzleY)
                         );

            rect.Offset (deltaX);
            rect.Offset (deltaY);

            return new RotatedRectangle (rect, rads);
        }

        public override void Explode ()
        {
            Sound.ExplosionSounds.TankExplosion.Play ();
            GenerateExplosion (Position.CollisionRectangle.Center, new Vector2 (Position.CollisionRectangle.Width / 3));

            var random = new Random ();
            for (int explosionNr = 0; explosionNr < 5; explosionNr++)
            {
                var x = Position.CollisionRectangle.X + random.Next (Position.CollisionRectangle.Width);
                var y = Position.CollisionRectangle.Y + random.Next (Position.CollisionRectangle.Height);

                var width = random.Next (16, (int)(Position.CollisionRectangle.Width / 1.5));
                var height = random.Next (16, (int)(Position.CollisionRectangle.Height / 1.5));

                Sound.ExplosionSounds.ExplosionEcho.Play ();
                GenerateExplosion (new Point (x, y), new Vector2 (width, height));
            }
        }

        public override void Render ()
        {
            const int tankSpriteImageColumns = 1;
            const int tankSpriteImageRows = 2;
            const int tankIndex = 1;
            const int tankTurrel = 0;

            var texture = GameData.MappedTextures [GetType ()];
            var position = GameData.Map.CalculateViewportCoordinates (Position.CollisionRectangle.Center.ToVector2 (),
                                                                      GameData.MapScale);

            var tankRect = Renderer.GetSpriteFromSpriteImage (texture,
                                                              tankIndex,
                                                              tankSpriteImageRows,
                                                              tankSpriteImageColumns);
            GameData.SpriteBatch.Draw (
                texture,
                position: position,
                sourceRectangle: tankRect,
                origin: new Vector2 (tankRect.Width / 2, tankRect.Height / 2),
                rotation: Position.Rotation,
                scale: GameData.MapScale
            );

            var turrelRect = Renderer.GetSpriteFromSpriteImage (texture,
                                                                tankTurrel,
                                                                tankSpriteImageRows,
                                                                tankSpriteImageColumns);
            GameData.SpriteBatch.Draw (
                texture,
                position: position,
                sourceRectangle: turrelRect,
                origin: new Vector2 (tankRect.Width / 2, tankRect.Height / 2),
                rotation: TurretRotation.ToRadians (),
                scale: GameData.MapScale
            );
        }
    }
}

