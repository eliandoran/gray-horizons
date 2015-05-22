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
    /// Indicates that a <see cref="GrayHorizons.Logic.GameAction"/> can be executed continously while the <see cref="GrayHorizons.Input.InputBinding"/> it is bound to is active.  
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class AllowContinuousPressAttribute: Attribute
    {
        // No implementation needed.
    }
}

