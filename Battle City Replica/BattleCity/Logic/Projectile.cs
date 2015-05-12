using System;
using System.ComponentModel;
using System.Diagnostics;
using BattleCity.Logic;
using Microsoft.Xna.Framework;
using BattleCity.Extensions;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    /// <summary>
    /// Represents an in-game projectile entity.
    /// </summary>
    [MappedTextures ("Projectile")]
    public class Projectile: Entity
    {
        /// <summary>
        /// Gets or sets the damage this instance of this <see cref="BattleCity.Entities.Projectile"/> does on impact with derivates of <see cref="BattleCity.Entities.ObjectBase"/>.
        /// </summary>
        /// <value>The damage value of this instance.</value>
        [DefaultValue (1)]
        public int Damage { get; set; }

        /// <summary>
        /// Gets or sets the time it takes for a <see cref="BattleCity.Entities.Tank" /> to fire another <see cref="BattleCity.Entities.Projectile"/>.
        /// </summary>
        /// <value>The cooltime penalty.</value>
        public TimeSpan CooltimePenalty { get; set; }

        /// <summary>
        /// Gets or sets the initial size of the explosion.
        /// </summary>
        /// <value>The initial size of the explosion.</value>
        public Vector2 InitialExplosionSize { get; set; }

        public Tank OwnerTank { get; set; }

        public TimeSpan OwnerTankImmunity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.Projectile"/> class.
        /// </summary>
        public Projectile ()
        {
            // Default values:
            Health = 1;
            Damage = 1;
            CooltimePenalty = new TimeSpan (0, 0, 1);
            InitialExplosionSize = new Vector2 (16, 16);
            DefaultSize = new Point (45, 7);
            HasCollision = true;
            IsInvincible = false;

            DestructionDelay = TimeSpan.Zero;
            OwnerTankImmunity = TimeSpan.FromMilliseconds (0x4c4b40);
        }


        public override void Update (
            TimeSpan gameTime)
        {
            base.Update (gameTime);

            var immunityLeft = OwnerTankImmunity.Subtract (gameTime);
            if (immunityLeft.TotalMilliseconds > 0)
                OwnerTankImmunity = immunityLeft;
            else
                OwnerTankImmunity = TimeSpan.Zero;

            const int step = 4;
            var rads = Rotation.FromRadians (Position.Rotation).OffsetBy (90).ToRadians ();

            Point delta = GetDelta (rads, step);
            Position.CollisionRectangle.Offset (delta);

            var obstacles = GameData.Map.SearchMapObjects (Position);
            foreach (ObjectBase obstacle in obstacles)
            {
                if (obstacle != this && !obstacle.IsBeingDestroyed)
                {
                    if (obstacle == OwnerTank && OwnerTankImmunity.TotalMilliseconds > 0)
                    {
                        #if DEBUG
                        Debug.WriteLine (
                            "Owner tank immunity ({0}ms left).".FormatWith (OwnerTankImmunity.TotalMilliseconds),
                            "PROJECTILE");
                        #endif
                        continue;
                    }

                    Debug.WriteLine ("Hit object of type <{0}>.".FormatWith (obstacle.GetType ().Name), "PROJECTILE");
                    obstacle.WasHitByProjectile (this);
                }
            }                

            if (!GameData.Map.IntersectsMap (Position))
                Destroy ();
        }

        public override void Explode ()
        {
            Sound.ExplosionSounds.StandardExplosion.Play ();
            base.Explode ();
        }
    }
}

