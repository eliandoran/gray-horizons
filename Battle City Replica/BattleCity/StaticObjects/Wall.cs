using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using BattleCity.Logic;
using BattleCity.Entities;
using BattleCity.Attributes;

namespace BattleCity.StaticObjects
{
    /// <summary>
    /// Represents a wall on the map.
    /// </summary>
    [MappedTextures ("Wall")]
    public class Wall: StaticObject
    {
        /// <summary>
        /// Gets a matrix of segments of the wall, where True means it is exists, False otherwise.
        /// </summary>
        /// <value>The segments matrix.</value>
        [XmlElement ("Segment")]
        public List<Segment> Segments { get; set; }

        /// <summary>
        /// Gets a list of four intact segments (a full wall).
        /// </summary>
        /// <value>The default segments.</value>
        public static List<Segment> DefaultSegments
        {
            get
            {
                return new List<Segment> () {
                    new Segment (),
                    new Segment (),
                    new Segment (),
                    new Segment ()
                };
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.Wall"/> class with all segments intact.
        /// </summary>
        public Wall () : this (
                new List<Segment> ())
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.Wall"/> class.
        /// </summary>
        /// <param name="segments">A matrix containing the segments integrity of this wall.</param>
        public Wall (
            List<Segment> segments)
        {
            Segments = segments;
            HasCollision = true;
            IsInvincible = false;
            Health = 1;
        }

        public override void WasHitByProjectile (
            Projectile hitter)
        {
            base.WasHitByProjectile (hitter);
        }

        public override void Explode ()
        {
            Sound.ExplosionSounds.WallExplosion.Play ();
            base.Explode ();
        }
    }
}

