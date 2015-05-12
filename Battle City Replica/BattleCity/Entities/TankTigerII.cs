using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\Tiger-II")]
    public class TankTigerII: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.TankTigerII"/> class.
        /// </summary>
        public TankTigerII ()
        {
            DefaultSize = new Point (276, 106);
        }
    }
}

