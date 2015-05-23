namespace GrayHorizons.Attributes
{
    using System;
    using System.Collections.Generic;
    using GrayHorizons.Extensions;

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class MappedSoundsAttribute: Attribute
    {
        readonly List<String> soundNames;

        public MappedSoundsAttribute(
            params string[] soundNames)
        {
            this.soundNames = new List<string>(soundNames);
        }

        public List<String> SoundNames
        {
            get
            {
                return soundNames;
            }
        }

        public override string ToString()
        {
            return "[MappedSoundsAttribute: SoundNames={0}]".FormatWith(
                String.Join("|", SoundNames.ToArray()));
        }
    }
}

