using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;

namespace GrayHorizons.Entities.Tanks
{
    [MappedTextures("Tanks\\E-100")]
    public class TankE100: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankE100"/> class.
        /// </summary>
        public TankE100()
        {
            DefaultSize = new Point(193, 82);
            MuzzlePosition = new Vector2(190, 35);
        }
    }
}

