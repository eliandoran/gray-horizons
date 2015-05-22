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

namespace GrayHorizons.Attributes
{
    /// <summary>
    /// Indicates that a <see cref="GrayHorizons.Sound.SoundEffect"/> should be automatically loaded by the <see cref="GrayHorizons.Loader"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class SoundAutoLoadAttribute: Attribute
    {
        // No implementation needed.
    }
}

