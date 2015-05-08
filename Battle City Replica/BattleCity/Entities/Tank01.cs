using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\01")]
    public class Tank01: Tank
    {
        public Tank01 ()
        {
            DefaultSize = new Point (193, 82);
        }
    }
}

