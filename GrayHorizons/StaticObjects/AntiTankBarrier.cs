using System;
using GrayHorizons.Attributes;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;

namespace GrayHorizons.StaticObjects
{
    [MappedTextures(@"AntiTank")]
    public class AntitankBarrier: StaticObject
    {
        public AntitankBarrier()
        {
            DefaultSize = new Point(80, 76);
            HasCollision = true;
        }
    }
}

