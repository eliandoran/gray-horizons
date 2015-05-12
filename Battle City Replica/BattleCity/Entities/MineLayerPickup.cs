using System;
using BattleCity.Attributes;
using BattleCity.Logic;
using Microsoft.Xna.Framework;

namespace BattleCity.Entities
{
    [MappedTextures (@"Vehicles\Pickup")]
    public class MineLayerPickup: Vehicle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.MineLayerPickup"/> class.
        /// </summary>
        public MineLayerPickup ()
        {
            DefaultSize = new Point (150, 62);
            Acceleration = 0.025f;
            AxisPosition = new Point (36, 30);
        }
    }
}

