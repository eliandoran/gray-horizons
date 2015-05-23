namespace GrayHorizons.Entities.Tanks
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using GrayHorizons.ThirdParty;
    using Microsoft.Xna.Framework;

    [MappedTextures("Tanks\\VK.3601h")]
    public class TankVK3601h: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankVK3601h"/> class.
        /// </summary>
        public TankVK3601h()
        {
            DefaultSize = new Point(229, 113);
            MuzzleRectangle = new RotatedRectangle(new Rectangle(228, 52, 10, 10), 0);
            MaximumHealth = 8;
        }
    }
}

