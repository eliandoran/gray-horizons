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
using System;
using GrayHorizons.Attributes;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities.Cars
{
    [MappedTextures(@"Vehicles\Car")]
    public class OrangeCar: Vehicle
    {
        public OrangeCar()
        {
            DefaultSize = new Point(154, 66);
            Acceleration = 0.005f;
            AxisPosition = new Point(36, 30);
            CanMoveOnSpot = true;
            CanBeRunOverByTank = true;
            Speed = 40;
        }
    }
}

