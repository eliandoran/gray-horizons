using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\Pz.Kpfw.IV-G-2")]
    public class TankPzKpfwB: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.TankPzKpfwB"/> class.
        /// </summary>
        public TankPzKpfwB ()
        {
            DefaultSize = new Point (239, 115);
        }
    }
}

