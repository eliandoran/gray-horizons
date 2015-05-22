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
using Microsoft.Xna.Framework;

namespace GrayHorizons.Entities.Projectiles
{
    [MappedTextures(@"Projectiles\MachineGunBullet")]
    public class MachineGunBullet: Projectile
    {
        public const int CooldownMilliseconds = 300;

        public MachineGunBullet()
        {
            CoolTimePenalty = TimeSpan.FromMilliseconds(CooldownMilliseconds);
            Speed = 15;
            Damage = 1;
            DefaultSize = new Point(18, 7);
        }
    }
}

