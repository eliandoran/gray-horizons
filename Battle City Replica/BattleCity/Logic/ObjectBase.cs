using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using BattleCity.Entities;
using BattleCity.ThirdParty;
using BattleCity.StaticObjects;
using BattleCity.Extensions;

namespace BattleCity.Logic
{
    /// <summary>
    /// Represents either an entity or a static object that can be placed on the map.
    /// </summary>
    public abstract class ObjectBase
    {
        /// <summary>
        /// Gets or sets the map which contains this entity.
        /// </summary>
        /// <value>The parent map.</value>
        [XmlIgnore ()]
        public GameData GameData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BattleCity.Logic.ObjectBase"/> has collision.
        /// </summary>
        /// <value><c>true</c> if this instance has collision; otherwise, <c>false</c>.</value>
        [XmlElement ("HasCollision"), DefaultValue (true)]
        public bool HasCollision { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="BattleCity.Logic.ObjectBase"/> is invincible.
        /// </summary>
        /// <value><c>true</c> if this instance is invincible; otherwise, <c>false</c>.</value>
        [XmlElement ("IsInvincible"), DefaultValue (false)]
        public bool IsInvincible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a projectile can pass through this <see cref="BattleCity.Logic.ObjectBase"/>.
        /// </summary>
        /// <value><c>true</c> if allow pass through; otherwise, <c>false</c>.</value>
        [XmlElement ("AllowPassThrough")]
        public bool AllowPassThrough { get; set; }

        /// <summary>
        /// Gets or sets the health of this object.
        /// </summary>
        /// <value>The health of the object.</value>
        [XmlElement ("Health"), DefaultValue (1)]
        public int Health { get; set; }

        /// <summary>
        /// The location of this object on the game map.
        /// </summary>
        [XmlElement ("Position")]
        public RotatedRectangle Position;

        public Point DefaultSize { get; set; }

        public TimeSpan DestructionTimeLeft { get; set; }

        public TimeSpan DestructionDelay { get; set; }

        public bool IsBeingDestroyed { get; set; }

        /// <summary>
        /// This procedure should be called each time the object is hit by a projectile, to manage its health and destruction.
        /// </summary>
        /// <param name="hitter">The projectile which hit this object.</param>
        public virtual void WasHitByProjectile (
            Projectile hitter)
        {
            #if DEBUG
            Debug.WriteLine ("<{0}> was hit by <{1}>.".FormatWith (ToString (), hitter), "HIT");
            #endif

            if (HasCollision)
            {
                if (!IsInvincible)
                {
                    if (Health - hitter.Damage > 0)
                        Health -= hitter.Damage;
                    else
                        Explode ();

                    if (!AllowPassThrough)
                        hitter.GenerateExplosion ();
                }
                else
                {
                    hitter.Explode ();
                }
            }                
        }

        public virtual void Explode ()
        {
            GenerateExplosion (Position.CollisionRectangle.Center,
                new Vector2 (Position.Width,
                    Position.Height));
        }

        public void GenerateExplosion (
            Point sourceOfImpact,
            Vector2 explosionSize)
        {
            int x = sourceOfImpact.X - ((int)explosionSize.X / 2);
            int y = sourceOfImpact.Y - ((int)explosionSize.Y / 2);

            GameData.Map.StaticObjects.Add (new Explosion () {
                Position = new RotatedRectangle (new Rectangle (x, y, (int)explosionSize.X, (int)explosionSize.Y),
                    Position.Rotation),
                GameData = GameData
            });

            DestructionTimeLeft = DestructionDelay;
            IsBeingDestroyed = true;
        }

        public void GenerateExplosion ()
        {
            GenerateExplosion (Position.CollisionRectangle.Center, new Vector2 (32, 32));
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public void Destroy ()
        {
            GameData.Map.QueueRemoval (this);

            #if DEBUG
            Debug.WriteLine ("<{0}> was destroyed.".FormatWith (ToString ()), "DESTROY");
            #endif
        }

        /// <summary>
        /// It should be called when the game requires the <see cref="BattleCity.Logic.ObjectBase"/> to change its current state, i.e. a game tick has passed.
        /// </summary>
        public virtual void Update (
            TimeSpan gameTime)
        {
            if (IsBeingDestroyed)
            {
                Debug.WriteLine (DestructionTimeLeft.TotalMilliseconds);

                if (DestructionTimeLeft > DestructionDelay)
                {
                    DestructionTimeLeft = DestructionTimeLeft.Subtract (gameTime);
                }
                else
                {
                    Destroy ();
                    IsBeingDestroyed = false;
                }
            }
        }

        public abstract void Render ();
    }
}