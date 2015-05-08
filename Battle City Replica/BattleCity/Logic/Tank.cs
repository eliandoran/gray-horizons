using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using BattleCity.Logic;
using BattleCity.ThirdParty;
using BattleCity.Extensions;

namespace BattleCity.Entities
{
    /// <summary>
    /// Represents an in-game tank entity.
    /// </summary>
    public class Tank: Entity
    {
        AIBase ai;

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
                ai.ParentMap = ParentMap;
            }
        }

        public RotatedRectangle MuzzlePosition { get; set; }

        public TimeSpan Cooldown { get; set; }

        /// <summary>
        /// Configures a projectile to be fired at the tank's behest.
        /// </summary>
        public void Shoot(Projectile projectile)
        {
            if (Cooldown == TimeSpan.Zero)
            {
                var originX = (int)(Position.UpperLeftCorner ().X - Position.Origin.X);
                var originY = (int)(Position.UpperLeftCorner ().Y - Position.Origin.Y);

                var muzzlePos = GetMuzzleRotatedRectangle ();
                var rect = new RotatedRectangle (
                               new Rectangle (
                                   muzzlePos.CollisionRectangle.X,
                                   muzzlePos.CollisionRectangle.Y,
                                   projectile.DefaultSize.X,
                                   projectile.DefaultSize.Y), Position.Rotation);                

                projectile.Position = rect;

                ParentMap.QueueAddition (projectile);
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
        /// Initializes a new instance of the <see cref="BattleCity.Logic.Tank"/> class.
        /// </summary>
        public Tank ()
        {
            HasCollision = true;
            IsInvincible = false;

            Health = 1;
            Score = 0;
            AccelerationFactor = 0.05f;
            Acceleration = 0.05f;
        }

        /// <summary>
        /// Calls the underlying artificial intelligence or player to update the state of the tank.
        /// </summary>
        public override void Update(TimeSpan gameTime)
        {
            base.Update (gameTime);

            if (Cooldown > gameTime)
                Cooldown = Cooldown.Subtract (gameTime);
            else
                Cooldown = TimeSpan.Zero;

            if (AI != null)
                AI.NextStep ();
        }

        public static RotatedRectangle OffsetRotatedRectangle(RotatedRectangle rect,
                                                              Point offsetBy)
        {
            var newRect = rect;  
            var rads = rect.Rotation;

            var deltaX = new Point (
                             (int)(Math.Cos (rads) * offsetBy.X),
                             (int)(Math.Sin (rads) * offsetBy.X));
            var deltaY = new Point (
                (int)(Math.Sin (rads) * -offsetBy.Y),
                             (int)(Math.Cos (rads) * offsetBy.Y)
            ));

            newRect.CollisionRectangle.Offset (deltaX);
            newRect.CollisionRectangle.Offset (deltaY);
        }

        public RotatedRectangle GetMuzzleRotatedRectangle()
        {
            var rect = new Rectangle (
                           (int)(Position.UpperLeftCorner ().X),
                           (int)(Position.UpperLeftCorner ().Y),
                           MuzzlePosition.CollisionRectangle.Width,
                           MuzzlePosition.CollisionRectangle.Height
                       );                   
            var rads = Position.Rotation;

            var deltaX = new Point (
                             (int)(Math.Cos (rads) * MuzzlePosition.CollisionRectangle.X),
                             (int)(Math.Sin (rads) * MuzzlePosition.CollisionRectangle.X));
            var deltaY = new Point (
                             (int)(Math.Sin (rads) * -MuzzlePosition.CollisionRectangle.Y),
                             (int)(Math.Cos (rads) * MuzzlePosition.CollisionRectangle.Y)
                         );

            rect.Offset (deltaX);
            rect.Offset (deltaY);
            return new RotatedRectangle (rect, rads);
        }
    }
}

