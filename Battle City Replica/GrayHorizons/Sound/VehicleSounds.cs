using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class VehicleSounds
    {
        [MappedSounds(@"Sounds\\VehicleStarting")]
        public static SoundEffect Starting = new SoundEffect();
    }
}

