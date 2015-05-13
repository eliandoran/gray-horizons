using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities
{
    [MappedTextures ("Tanks\\KV-2")]
    public class TankKV2: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankKV2"/> class.
        /// </summary>
        public TankKV2 ()
        {
            DefaultSize = new Point (240, 118);
        }
    }
}

