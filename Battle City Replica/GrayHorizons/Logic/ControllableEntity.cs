using System;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;
using GrayHorizons.Extensions;

namespace GrayHorizons.Logic
{
    public abstract class ControllableEntity: Entity
    {

        public Rotation TurretRotation { get; set; }

        public RotatedRectangle TurretRect { get; set; }

        public int AmmoLeft { get; set; }

        int ammoCapacity;

        public TimeSpan CoolDown { get; set; }

        public int AmmoCapacity
        {
            get
            {
                return ammoCapacity;
            }
            set
            {
                ammoCapacity = value;
                AmmoLeft = value;
            }
        }

        public RotatedRectangle MuzzleRectangle { get; set; }

        Vector2 muzzlePosition;

        public Vector2 MuzzlePosition
        {
            get
            {
                return muzzlePosition;
            }
            protected set
            {
                muzzlePosition = value;
                MuzzleRectangle = new RotatedRectangle(new Rectangle((int)muzzlePosition.X,
                        (int)muzzlePosition.Y,
                        10,
                        10),
                    0);
            }
        }

        public virtual void Use()
        {
            
        }

        public RotatedRectangle GetMuzzleRotatedRectangle()
        {    
            if (MuzzleRectangle.IsNull())
                return null;

            MuzzleRectangle.Rotation = TurretRotation.ToRadians();
            TurretRect.Rotation = TurretRotation.ToRadians();
            var muzzleX = ((MuzzleRectangle.IsNotNull()) ? MuzzleRectangle.X : 0);
            var muzzleY = ((MuzzleRectangle.IsNotNull()) ? MuzzleRectangle.Y : 0);

            var rect = new Rectangle(
                           (int)(TurretRect.UpperLeftCorner().X),
                           (int)(TurretRect.UpperLeftCorner().Y),
                           ((MuzzleRectangle.IsNotNull()) ? MuzzleRectangle.CollisionRectangle.Width : 0),
                           ((MuzzleRectangle.IsNotNull()) ? MuzzleRectangle.CollisionRectangle.Height : 0)
                       );

            var rads = TurretRotation.ToRadians();

            var deltaX = new Point(
                             (int)(Math.Cos(rads) * muzzleX),
                             (int)(Math.Sin(rads) * muzzleX));
            var deltaY = new Point(
                             (int)(Math.Sin(rads) * -muzzleY),
                             (int)(Math.Cos(rads) * muzzleY)
                         );

            rect.Offset(deltaX);
            rect.Offset(deltaY);

            return new RotatedRectangle(rect, rads);
        }
    }
}

