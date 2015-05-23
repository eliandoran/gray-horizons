namespace GrayHorizons.Entities.Tanks
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;

    [MappedTextures("Tanks\\Pz.Kpfw.IV-G")]
    public class TankPzKpfwA: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankPzKpfwA"/> class.
        /// </summary>
        public TankPzKpfwA()
        {
            DefaultSize = new Point(227, 110);
            MaximumHealth = 13;
        }
    }
}

