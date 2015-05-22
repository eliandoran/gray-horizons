using System;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using GrayHorizons.Entities;
using GrayHorizons.ThirdParty;
using GrayHorizons.StaticObjects;
using GrayHorizons.Extensions;
using Microsoft.Xna.Framework.Graphics;

namespace GrayHorizons.Logic
{
    public class CollideEventArgs: EventArgs
    {
        readonly ObjectBase collidedWith;

        public ObjectBase CollidedWith
        {
            get
            {
                return collidedWith;
            }
        }

        public bool PassThrough { get; set; }

        public CollideEventArgs(ObjectBase collidedWith, bool passThrough)
        {
            this.collidedWith = collidedWith;
            PassThrough = passThrough;
        }

        public CollideEventArgs(ObjectBase collidedWith)
            : this(collidedWith, false)
        {
         
        }
    }


    /// <summary>
    /// Represents either an entity or a static object that can be placed on the map.
    /// </summary>
    public abstract class ObjectBase: IRenderable
    {
        public event EventHandler<CollideEventArgs> Collided;

        /// <summary>
        /// Gets or sets the map which contains this entity.
        /// </summary>
        /// <value>The parent map.</value>
        [XmlIgnore]
        public GameData GameData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GrayHorizons.Logic.ObjectBase"/> has collision.
        /// </summary>
        /// <value><c>true</c> if this instance has collision; otherwise, <c>false</c>.</value>
        [XmlElement("HasCollision"), DefaultValue(true)]
        public bool HasCollision { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GrayHorizons.Logic.ObjectBase"/> is invincible.
        /// </summary>
        /// <value><c>true</c> if this instance is invincible; otherwise, <c>false</c>.</value>
        [XmlElement("IsInvincible"), DefaultValue(false)]
        public bool IsInvincible { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a projectile can pass through this <see cref="GrayHorizons.Logic.ObjectBase"/>.
        /// </summary>
        /// <value><c>true</c> if allow pass through; otherwise, <c>false</c>.</value>
        [XmlElement("AllowPassThrough")]
        public bool AllowPassThrough { get; set; }

        /// <summary>
        /// Gets or sets the health of this object.
        /// </summary>
        /// <value>The health of the object.</value>
        [XmlElement("Health"), DefaultValue(1)]
        public int Health { get; set; }

        int maximumHealth;

        public int MaximumHealth
        {
            get
            {
                return maximumHealth;
            }
            set
            {
                maximumHealth = value;
                Health = value;
            }
        }

        public double HealthPercentage
        {
            get
            {
                return (double)Health / (double)MaximumHealth;
            }
        }

        [XmlIgnore]
        public Texture2D CustomTexture { get; set; }

        public Rectangle? CustomTextureCrop { get; set; }

        /// <summary>
        /// The location of this object on the game map.
        /// </summary>
        [XmlElement("Position")]
        public RotatedRectangle Position;

        public Point? Location
        {
            get
            {
                return Position.IsNull() ? null : (Point?)Position.CollisionRectangle.Location;
            }

            set
            {
                if (value.HasValue)
                {
                    if (Position.IsNull())
                        Position = value.HasValue ? new RotatedRectangle(new Rectangle(value.Value.X, value.Value.Y, DefaultSize.X, DefaultSize.Y), 0) : null;
                    else
                    {
                        Position.CollisionRectangle.X = value.Value.X;
                        Position.CollisionRectangle.Y = value.Value.Y;
                    }
                }
            }
        }

        public float? Orientation
        {
            get
            {
                return Position.IsNull() ? null : (float?)Position.Rotation;
            }
            set
            {
                if (value.HasValue && Position.IsNotNull())
                    Position.Rotation = value.Value;
            }
        }

        public Point DefaultSize { get; set; }

        public TimeSpan DestructionTimeLeft { get; set; }

        public TimeSpan DestructionDelay { get; set; }

        public bool IsBeingDestroyed { get; set; }

        public Color? MiniMapColor { get; set; }

        /// <summary>
        /// This procedure should be called each time the object is hit by a projectile, to manage its health and destruction.
        /// </summary>
        /// <param name="hitter">The projectile which hit this object.</param>
        public virtual void WasHitByProjectile(
            Projectile hitter)
        {
            Debug.WriteLine("<{0}> was hit by <{1}>.".FormatWith(ToString(), hitter), "HIT");

            if (HasCollision)
            {
                if (!IsInvincible)
                {
                    if (Health - hitter.Damage >= 0)
                        Health -= hitter.Damage;
                    else
                        Explode();

                    if (!AllowPassThrough)
                        hitter.GenerateExplosion();
                }
                else
                {
                    hitter.Explode();
                }
            }                
        }

        public virtual void Explode()
        {
            GenerateExplosion(Position.CollisionRectangle.Center,
                new Vector2(Position.Width,
                    Position.Height));
        }

        public void GenerateExplosion(
            Point sourceOfImpact,
            Vector2 explosionSize)
        {
            int x = sourceOfImpact.X - ((int)explosionSize.X / 2);
            int y = sourceOfImpact.Y - ((int)explosionSize.Y / 2);

            GameData.Map.StaticObjects.Add(new Explosion
                {
                    Position = new RotatedRectangle(new Rectangle(x, y, (int)explosionSize.X, (int)explosionSize.Y),
                        Position.Rotation),
                    GameData = GameData
                });

            DestructionTimeLeft = DestructionDelay;
            IsBeingDestroyed = true;
        }

        public void GenerateExplosion()
        {
            GenerateExplosion(Position.CollisionRectangle.Center, new Vector2(32, 32));
        }

        /// <summary>
        /// Destroy this instance.
        /// </summary>
        public virtual void Destroy()
        {
            GameData.Map.QueueRemoval(this);

            Debug.WriteLine("<{0}> was destroyed.".FormatWith(ToString()), "DESTROY");
        }

        /// <summary>
        /// It should be called when the game requires the <see cref="GrayHorizons.Logic.ObjectBase"/> to change its current state, i.e. a game tick has passed.
        /// </summary>
        public virtual void Update(
            TimeSpan gameTime)
        {
            if (IsBeingDestroyed)
            {
                Debug.WriteLine(DestructionTimeLeft.TotalMilliseconds);

                if (DestructionTimeLeft > DestructionDelay)
                {
                    DestructionTimeLeft = DestructionTimeLeft.Subtract(gameTime);
                }
                else
                {
                    Destroy();
                    IsBeingDestroyed = false;
                }
            }
        }

        internal virtual void OnCollide(CollideEventArgs e)
        {
            if (Collided.IsNotNull())
            {
                Collided(this, e);
            }
        }

        public abstract void Render();

        public abstract void RenderHud();
    }
}