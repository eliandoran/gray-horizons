using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\05")]
    public class Tank05: Tank
    {
        public Tank05 ()
        {
            DefaultSize = new Point (165, 77);
        }
    }
}

