using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities
{
    [MappedTextures ("Tanks\\Pz.Kpfw.IV-G-2")]
    public class TankPzKpfwB: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankPzKpfwB"/> class.
        /// </summary>
        public TankPzKpfwB ()
        {
            DefaultSize = new Point (239, 115);
        }
    }
}

