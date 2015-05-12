using System;
using System.Collections.Generic;

namespace BattleCity.Attributes
{
    [AttributeUsage (AttributeTargets.Field)]
    public class MappedSoundsAttribute: Attribute
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

