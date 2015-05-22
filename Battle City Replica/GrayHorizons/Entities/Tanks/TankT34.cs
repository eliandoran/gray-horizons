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
using GrayHorizons.Entities;
using Microsoft.Xna.Framework;
using GrayHorizons.Logic;

namespace GrayHorizons.Entities.Tanks
{
    [MappedTextures("Tanks\\T34")]
    public class Tank05: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.Tank05"/> class.
        /// </summary>
        public Tank05()
        {
            DefaultSize = new Point(165, 77);
            MaximumHealth = 6;
        }
    }
}

