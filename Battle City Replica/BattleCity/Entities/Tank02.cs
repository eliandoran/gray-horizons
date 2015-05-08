using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\02")]
    public class Tank02: Tank
    {
        public Tank02 ()
        {
            DefaultSize = new Point (240, 118);
        }
    }
}

