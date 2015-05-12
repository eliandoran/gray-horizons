using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\KV-2")]
    public class TankKV2: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.TankKV2"/> class.
        /// </summary>
        public TankKV2 ()
        {
            DefaultSize = new Point (240, 118);
        }
    }
}

