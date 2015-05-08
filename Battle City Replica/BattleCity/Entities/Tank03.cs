using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\03")]
    public class Tank03: Tank
    {
        public Tank03 ()
        {
            DefaultSize = new Point (256, 95);
        }
    }
}

