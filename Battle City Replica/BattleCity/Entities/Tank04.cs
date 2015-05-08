using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\04")]
    public class Tank04: Tank
    {
        public Tank04 ()
        {
            DefaultSize = new Point (227, 110);
        }
    }
}

