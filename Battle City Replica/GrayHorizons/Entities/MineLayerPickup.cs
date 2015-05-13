using GrayHorizons.Attributes;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities
{
    [MappedTextures (@"Vehicles\Pickup")]
    public class MinelayerPickup: Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.MineLayerPickup"/> class.
        /// </summary>
        public MinelayerPickup ()
        {
            DefaultSize = new Point (160, 66);
            Acceleration = 0.025f;
            AxisPosition = new Point (36, 30);
            CanMoveOnSpot = false;
        }
    }
}

