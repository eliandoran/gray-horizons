using System;
using BattleCity.Logic;
using BattleCity.Entities;
using Microsoft.Xna.Framework;
using BattleCity.Attributes;

namespace BattleCity.Entities
{
    [MappedTextures ("Tanks\\08")]
    public class Tank08: Tank
    {
        public Tank08 ()
        {
            DefaultSize = new Point (229, 113);
        }
    }
}

