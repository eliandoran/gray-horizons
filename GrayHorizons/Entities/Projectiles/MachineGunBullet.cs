namespace GrayHorizons.Entities.Projectiles
{
    using System;
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;

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

