﻿using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;

namespace GrayHorizons.Entities.Tanks
{
    [MappedTextures("Tanks\\Tiger-II")]
    public class TankTigerII: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankTigerII"/> class.
        /// </summary>
        public TankTigerII()
        {
            DefaultSize = new Point(276, 106);
        }
    }
}

