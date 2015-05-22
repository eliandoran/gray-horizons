/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using System;
using System.Diagnostics;
using GrayHorizons.Attributes;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace GrayHorizons.Entities
{
    [MappedTextures("Soldier")]
    public class Soldier: ControllableEntity
    {
        const int moveStatesCount = 7;
        int currentMoveState = 0;
        int spriteColumn = 0;

        bool isMoving, firstTimeMoving;
        TimeSpan moveStateTime, currentMoveStateTime;
        TimeSpan currentIsMovingTime, isMovingTime;

        bool isShooting, firstTimeShooting;
        TimeSpan shootingStateTime, currentShootingStateTime;
        TimeSpan currentIsShootingTime, isShootingTime;

        SoundEffectInstance footstepsSound, firingSound;
        RotatedRectangle usageRect;
        float lastMouseAngle;

        public Soldier()
        {
            Health = 1;
            AccelerationFactor = 0.03f;
            Acceleration = 0.15f;
            DefaultSize = new Point(75, 50);
            moveStateTime = TimeSpan.FromMilliseconds(75);
            isMovingTime = TimeSpan.FromMilliseconds(350);
            isShootingTime = TimeSpan.FromMilliseconds(350);
            HasCollision = true;
            Speed = 6;
            MuzzlePosition = new Vector2(74, 40);
            MaximumHealth = 2;
            AmmoCapacity = 50;
        }

        public override bool Move(
            MoveDirection direction,
            bool noClip)
        {
            TurretRect.CollisionRectangle.X = Position.CollisionRectangle.X;
            TurretRect.CollisionRectangle.Y = Position.CollisionRectangle.Y;
            TurretRect.Rotation = Position.Rotation;

            isMoving = true;
            currentIsMovingTime = isMovingTime;
            return base.Move(direction, noClip);
        }

        public override void Shoot()
        {
            if (CoolDown == TimeSpan.Zero)
            {
                var projectile = new Projectiles.MachineGunBullet();

                if (AmmoLeft <= 0)
                {
                    Sound.TankSounds.NoAmmo.Play();
                    CoolDown = projectile.CoolTimePenalty;
                    return;
                }

                isShooting = true;
                currentIsShootingTime = isShootingTime;

                if (firstTimeShooting)
                {
                    firingSound = Sound.SoldierSounds.Firing.GetInstance();
                    firingSound.IsLooped = true;
                    firingSound.Play();
                    firstTimeShooting = false;
                }

                var xOffset = new Point(projectile.DefaultSize.X, 0);
                var yOffset = new Point(0, projectile.DefaultSize.Y);
                var muzzlePos = GetMuzzleRotatedRectangle();
                if (muzzlePos.IsNull())
                    return;

                var rect = muzzlePos.Offset(xOffset).Offset(yOffset);

                Debug.WriteLine("ROTATION: " + Rotation.FromRadians(TurretRect.Rotation));

                projectile.Position = rect;
                projectile.Owner = this;

//                var explosion = new Explosion();
//                explosion.Position = new RotatedRectangle(muzzlePos.CollisionRectangle, muzzlePos.Rotation);
//                GameData.Map.QueueAddition(explosion);
                GameData.Map.QueueAddition(projectile);
                CoolDown = projectile.CoolTimePenalty;
                AmmoLeft--;

                #if DEBUG
                Debug.WriteLine("<{0}> shot with <{1}>.".FormatWith(ToString(), projectile), "SHOOT");
                #endif
            }
        }

        public override void Render()
        {
            var texture = GameData.MappedTextures[GetType()];
            var position = GameData.Map.CalculateViewportCoordinates(
                               Position.CollisionRectangle.Center.ToVector2(),
                               GameData.MapScale);

            var size = new Size(100, 90);
            var origin = new Point(29, 22);
            int x = spriteColumn * size.Width;
            int y = currentMoveState * size.Height;
            var rect = new Rectangle(x, y, size.Width, size.Height);

            GameData.ScreenManager.SpriteBatch.Draw(
                texture,
                sourceRectangle: rect,
                position: position,
                origin: origin.ToVector2(),
                rotation: Position.Rotation,
                scale: GameData.MapScale
            );

            if (usageRect.IsNull())
                return;

            position = GameData.Map.CalculateViewportCoordinates(
                usageRect.CollisionRectangle.Location.ToVector2(),
                GameData.MapScale);

            if (GameData.DebuggingSettings.ShowGuides)
            {
                var blankTexture = GameData.ContentManager.Load<Texture2D>("blank");
                GameData.ScreenManager.SpriteBatch.Draw(
                    blankTexture,
                    destinationRectangle: new Rectangle(
                        (int)position.X,
                        (int)position.Y,
                        usageRect.CollisionRectangle.Width,
                        usageRect.CollisionRectangle.Height),
                    color: Color.Black * .25f,
                    rotation: usageRect.Rotation,
                    scale: GameData.MapScale
                );
            }
        }

        public override void Update(
            TimeSpan gameTime)
        {
            if (TurretRotation.IsNotNull())
            {
                var mouseAngle = TurretRotation.ToRadians();
                if (Math.Abs(mouseAngle - lastMouseAngle) > .025)
                {
                    Position.Rotation = mouseAngle;
                    lastMouseAngle = mouseAngle;
                }
            }

            usageRect = new RotatedRectangle(
                new Rectangle(
                    (int)Position.UpperRightCorner().X,
                    (int)Position.UpperRightCorner().Y,
                    25,
                    Position.CollisionRectangle.Height),
                Position.Rotation);

            var rads = Position.Rotation;
            var x = 15;
            var y = 0;
            var deltaX = new Point(
                             (int)(Math.Cos(rads) * x),
                             (int)(Math.Sin(rads) * x));
            var deltaY = new Point(
                             (int)(Math.Sin(rads) * -y),
                             (int)(Math.Cos(rads) * y)
                         );

            var rect = Position.CollisionRectangle;
            rect.Offset(deltaX);
            rect.Offset(deltaY);

            if (currentIsMovingTime > gameTime)
                currentIsMovingTime -= gameTime;
            else
            {
                isMoving = false;
                firstTimeMoving = true;
            }

            if (currentIsShootingTime > gameTime)
                currentIsShootingTime -= gameTime;
            else
            {
                isShooting = false;
                firstTimeShooting = true;

                if (firingSound.IsNotNull())
                    firingSound.Stop();
            }

            if (CoolDown > gameTime)
            {
                CoolDown -= gameTime;
            }
            else
            {
                CoolDown = TimeSpan.Zero;
            }

            if (isMoving)
            {
                if (firstTimeMoving)
                {
                    footstepsSound = Sound.SoldierSounds.Footsteps.PlayLooped();
                    firstTimeMoving = false;
                }

                if (currentMoveStateTime > gameTime)
                {
                    currentMoveStateTime -= gameTime;
                }
                else
                {
                    if (currentMoveState < moveStatesCount - 1)
                        currentMoveState++;
                    else
                        currentMoveState = 1;
                    currentMoveStateTime = moveStateTime;
                }
            }
            else
            {
                currentMoveState = 0;

                if (footstepsSound.IsNotNull())
                    footstepsSound.Stop();
            }         

            spriteColumn = (isShooting ? (isMoving ? 1 : 2) : 0);

            base.Update(gameTime);
        }

        public override void Use()
        {
            foreach (ObjectBase obj in GameData.Map.SearchMapObjects (usageRect))
            {
                if (obj == this)
                    continue;

                var vehicle = obj as Vehicle;
                if (vehicle.IsNotNull() && vehicle.CanBeBoarded)
                {
                    Debug.WriteLine("[USE] Identified vehicle of type <{0}>.".FormatWith(vehicle.GetType()));

                    isMoving = false;
                    vehicle.BoardPassenger(this);

                    if (GameData.ActivePlayer.AssignedEntity == this)
                    {
                        GameData.Map.CenterViewportAt(vehicle);

                        vehicle.Moved += (
                            sender,
                            e) => GameData.Map.CenterViewportAt(vehicle);
                        GameData.ActivePlayer.AssignedEntity = vehicle;

                        Debug.WriteLine("[USE] Player's assigned entity was changed to <{0}>.".FormatWith(vehicle.GetType()));
                    }

                    return;
                }
            }

            Debug.WriteLine("[USE] No suitable objects found.");
        }
    }
}

