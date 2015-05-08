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
        }


        public override void Update(TimeSpan gameTime)
        {
            const int step = 2;
            var rads = Rotation.FromRadians (Position.Rotation).OffsetBy (90).ToRadians ();
            Point delta = GetDelta (rads, step);
            Position.CollisionRectangle.Offset (delta);

            var obstacles = ParentMap.SearchStaticObject (Position);
            foreach (StaticObject obstacle in obstacles)
            {
                Debug.WriteLine ("Hit object of type <{0}>.".FormatWith (obstacle.GetType ().Name), "PROJECTILE");
                obstacle.WasHitByProjectile (this);
            }

            if (!ParentMap.IntersectsMap (Position))
                Destroy ();
        }
    }
}

