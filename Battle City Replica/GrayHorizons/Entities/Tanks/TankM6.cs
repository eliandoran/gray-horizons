using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;

namespace GrayHorizons.Entities.Tanks
{
    [MappedTextures("Tanks\\M-6")]
    public class TankM6: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankM6"/> class.
        /// </summary>
        public TankM6()
        {
            DefaultSize = new Point(256, 95);
        }
    }
}

