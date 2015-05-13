using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities
{
    [MappedTextures ("Tanks\\Pz.Kpfw.IV-G")]
    public class TankPzKpfwA: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankPzKpfwA"/> class.
        /// </summary>
        public TankPzKpfwA ()
        {
            DefaultSize = new Point (227, 110);
        }
    }
}

