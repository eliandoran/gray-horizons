using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\06")]
    public class Tank06: Tank
    {
        public Tank06 ()
        {
            DefaultSize = new Point (276, 106);
        }
    }
}

