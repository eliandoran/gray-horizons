using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities
{
    [MappedTextures ("Tanks\\E-100")]
    public class TankE100: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankE100"/> class.
        /// </summary>
        public TankE100 ()
        {
            DefaultSize = new Point (193, 82);
            MuzzlePosition = new GrayHorizons.ThirdParty.RotatedRectangle (new Rectangle (190, 35, 10, 10), 0);
        }
    }
}

