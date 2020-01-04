namespace GrayHorizons.Entities.Tanks
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Entities;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;

    [MappedTextures("Tanks\\Tiger-II")]
    public class TankTigerII: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankTigerII"/> class.
        /// </summary>
        public TankTigerII()
        {
            DefaultSize = new Point(276, 106);
            MaximumHealth = 6;
        }
    }
}

