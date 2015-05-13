using System;
using System.Collections.Generic;

namespace GrayHorizons.Attributes
{
    [AttributeUsage (AttributeTargets.Field)]
    public sealed class MappedSoundsAttribute: Attribute
    {
        readonly List<String> soundNames;

        public MappedSoundsAttribute (
            params string[] soundNames)
        {
            this.soundNames = new List<string> (soundNames);
        }

        public List<String> SoundNames
        {
            get
            {
                return soundNames;
            }
        }
    }
}

