using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\07")]
    public class Tank07: Tank
    {
        public Tank07 ()
        {
            DefaultSize = new Point (239, 115);
        }
    }
}

