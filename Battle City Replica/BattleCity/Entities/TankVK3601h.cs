using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\VK.3601h")]
    public class TankVK3601h: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.TankVK3601h"/> class.
        /// </summary>
        public TankVK3601h ()
        {
            DefaultSize = new Point (229, 113);
        }
    }
}

