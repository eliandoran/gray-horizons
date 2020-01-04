namespace GrayHorizons.Entities.Tanks
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;

    [MappedTextures("Tanks\\T34")]
    public class Tank05: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.Tank05"/> class.
        /// </summary>
        public Tank05()
        {
            DefaultSize = new Point(165, 77);
            MaximumHealth = 6;
        }
    }
}

