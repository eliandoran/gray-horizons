using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class ExplosionSounds
    {
        [MappedSounds(@"Sounds\\WallExplosion")]
        public static SoundEffect WallExplosion { get; internal set; }

        [MappedSounds(@"Sounds\\TankExplosion")]
        public static SoundEffect TankExplosion { get; internal set; }

        [MappedSounds(@"Sounds\\ExplosionEcho")]
        public static SoundEffect ExplosionEcho { get; internal set; }

        [MappedSounds(@"Sounds\\StandardExplosion")]
        public static SoundEffect StandardExplosion { get; internal set; }
    }
}

