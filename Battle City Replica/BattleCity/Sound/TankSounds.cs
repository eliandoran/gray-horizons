using System;
using BattleCity.Attributes;

namespace BattleCity.Sound
{
    [SoundAutoLoad]
    public static class TankSounds
    {
        [MappedSounds (
            @"Sounds\TankFiring01",
            @"Sounds\TankFiring02",
            @"Sounds\TankFiring03",
            @"Sounds\TankFiring04")]
        public static SoundEffect Firing = new SoundEffect ();

        [MappedSounds (@"Sounds\\TankIdle")]
        public static SoundEffect Idle = new SoundEffect ();

        [MappedSounds (@"Sounds\\TankMoving")]
        public static SoundEffect Moving = new SoundEffect ();
    }
}

