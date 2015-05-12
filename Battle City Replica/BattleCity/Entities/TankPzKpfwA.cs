using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\Pz.Kpfw.IV-G")]
    public class TankPzKpfwA: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.TankPzKpfwA"/> class.
        /// </summary>
        public TankPzKpfwA ()
        {
            DefaultSize = new Point (227, 110);
        }
    }
}

