using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\E-100")]
    public class TankE100: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Entities.TankE100"/> class.
        /// </summary>
        public TankE100 ()
        {
            DefaultSize = new Point (193, 82);
            MuzzlePosition = new BattleCity.ThirdParty.RotatedRectangle (new Rectangle (190, 35, 10, 10), 0);
        }
    }
}

