using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class TankSounds
    {
        [MappedSounds(
            @"Sounds/TankFiring01",
            @"Sounds/TankFiring02",
            @"Sounds/TankFiring03",
            @"Sounds/TankFiring04")]
        public static SoundEffect Firing { get; internal set; }

        [MappedSounds(@"Sounds/TankIdle")]
        public static SoundEffect Idle { get; internal set; }

        [MappedSounds(@"Sounds/TankMoving")]
        public static SoundEffect Moving { get; internal set; }

        [MappedSounds(@"Sounds/TankStarting")]
        public static SoundEffect Starting { get; internal set; }

        [MappedSounds(@"Sounds/TankNoAmmo")]
        public static SoundEffect NoAmmo { get; internal set; }
    }
}

