using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class MiscSounds
    {
        [MappedSounds(@"Sounds/AmmoCrate")]
        public static SoundEffect AmmoCrate { get; internal set; }
    }
}

