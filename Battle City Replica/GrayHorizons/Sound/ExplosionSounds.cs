using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class ExplosionSounds
    {
        [MappedSounds (@"Sounds\\WallExplosion")]
        public static SoundEffect WallExplosion = new SoundEffect ();

        [MappedSounds (@"Sounds\\TankExplosion")]
        public static SoundEffect TankExplosion = new SoundEffect ();

        [MappedSounds (@"Sounds\\ExplosionEcho")]
        public static SoundEffect ExplosionEcho = new SoundEffect ();

        [MappedSounds (@"Sounds\\StandardExplosion")]
        public static SoundEffect StandardExplosion = new SoundEffect ();
    }
}

