namespace GrayHorizons.Entities.Tanks
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;

    [MappedTextures("Tanks\\M-6")]
    public class TankM6: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankM6"/> class.
        /// </summary>
        public TankM6()
        {
            DefaultSize = new Point(256, 95);
            MaximumHealth = 10;
        }
    }
}

