﻿using GrayHorizons.Attributes;
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;
using GrayHorizons.ThirdParty;

namespace GrayHorizons.Entities
{
    [MappedTextures ("Tanks\\VK.3601h")]
    public class TankVK3601h: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankVK3601h"/> class.
        /// </summary>
        public TankVK3601h ()
        {
            DefaultSize = new Point (229, 113);
            MuzzlePosition = new RotatedRectangle (new Rectangle (228, 52, 10, 10), 0);
        }
    }
}

