using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\T34")]
    public class Tank05: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.Tank05"/> class.
        /// </summary>
        public Tank05 ()
        {
            DefaultSize = new Point (165, 77);
        }
    }
}

