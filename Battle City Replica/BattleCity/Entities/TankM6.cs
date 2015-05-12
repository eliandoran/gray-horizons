using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    //
    [MappedTextures ("Tanks\\M-6")]
    public class TankM6: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.TankM6"/> class.
        /// </summary>
        public TankM6 ()
        {
            DefaultSize = new Point (256, 95);
        }
    }
}

