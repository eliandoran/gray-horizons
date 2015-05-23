using System;
using GrayHorizons.Attributes;

namespace GrayHorizons.Sound
{
    [SoundAutoLoad]
    public static class UISounds
    {
        /// <summary>
        /// The menu select.
        /// Original source: http://www.freesound.org/people/DrMinky/sounds/166186/
        /// </summary>
        [MappedSounds(@"Sounds/MenuSelect")]
        public static SoundEffect MenuSelect { get; internal set; }
    }
}

