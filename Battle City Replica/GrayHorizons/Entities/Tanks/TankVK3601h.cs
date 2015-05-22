/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using GrayHorizons.Attributes;
using GrayHorizons.Logic;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities.Tanks
{
    [MappedTextures("Tanks\\VK.3601h")]
    public class TankVK3601h: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankVK3601h"/> class.
        /// </summary>
        public TankVK3601h()
        {
            DefaultSize = new Point(229, 113);
            MuzzleRectangle = new RotatedRectangle(new Rectangle(228, 52, 10, 10), 0);
            MaximumHealth = 8;
        }
    }
}

