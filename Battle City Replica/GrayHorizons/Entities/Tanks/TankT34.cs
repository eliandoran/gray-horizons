using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;

namespace GrayHorizons.Entities.Tanks
{
    [MappedTextures("Tanks\\T34")]
    public class Tank05: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.Tank05"/> class.
        /// </summary>
        public Tank05()
        {
            DefaultSize = new Point(165, 77);
        }
    }
}

