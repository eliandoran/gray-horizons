using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class SoldierSounds
    {
        // http://www.freesound.org/people/freefire66/sounds/175954/
        [MappedSounds (@"Sounds\\SoldierFootsteps")]
        public static SoundEffect Footsteps = new SoundEffect ();
    }
}

