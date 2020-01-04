namespace GrayHorizons.StaticObjects
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Events;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;

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

