/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using System;
using System.Collections.Generic;

namespace GrayHorizons.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
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
    }
}

