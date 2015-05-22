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
    [MappedTextures("Tanks\\KV-2")]
    public class TankKV2: Tank
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Entities.TankKV2"/> class.
        /// </summary>
        public TankKV2()
        {
            DefaultSize = new Point(240, 118);
            MaximumHealth = 12;
        }
    }
}

