using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class TankSounds
    {
        [MappedSounds(
            @"Sounds\TankFiring01",
            @"Sounds\TankFiring02",
            @"Sounds\TankFiring03",
            @"Sounds\TankFiring04")]
        public static SoundEffect Firing = new SoundEffect();

        [MappedSounds(@"Sounds\\TankIdle")]
        public static SoundEffect Idle = new SoundEffect();

        [MappedSounds(@"Sounds\\TankMoving")]
        public static SoundEffect Moving = new SoundEffect();

        [MappedSounds(@"Sounds\\TankStarting")]
        public static SoundEffect Starting = new SoundEffect();

        [MappedSounds(@"Sounds\\TankNoAmmo")]
        public static SoundEffect NoAmmo = new SoundEffect();
    }
}

