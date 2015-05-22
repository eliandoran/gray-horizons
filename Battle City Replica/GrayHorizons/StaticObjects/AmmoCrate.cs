using System;
using GrayHorizons.Logic;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework;
using GrayHorizons.Extensions;

namespace GrayHorizons.StaticObjects
{
    [MappedTextures("AmmoCrate")]
    public class AmmoCrate: StaticObject
    {
        public AmmoCrate()
        {
            DefaultSize = new Point(50, 20);
            HasCollision = true;
            Collided += AmmoCrate_Collided;
        }

        void AmmoCrate_Collided(object sender, CollideEventArgs e)
        {
            var obj = e.CollidedWith as ControllableEntity;
            if (obj.IsNotNull() && obj.AmmoLeft < obj.AmmoCapacity)
            {
                obj.AmmoLeft = obj.AmmoCapacity;
                Sound.MiscSounds.AmmoCrate.Play();
                Destroy();
            }
        }
    }
}

