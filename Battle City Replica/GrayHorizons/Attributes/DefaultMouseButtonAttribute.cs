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
using GrayHorizons.Logic;

namespace GrayHorizons.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class DefaultMouseButtonAttribute: Attribute
    {
        readonly MouseButtons mouseButton;

        /// <summary>
        /// Gets the key that will be bound implicitly to an <see cref="GrayHorizons.Input.InputBinding"/>.
        /// </summary>
        /// <value>The key.</value>
        public MouseButtons MouseButton
        {
            get
            {
                return mouseButton;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Attributes.DefaultKeyAttribute"/> class.
        /// </summary>
        /// <param name="key">The key to bind.</param>
        public DefaultMouseButtonAttribute(
            MouseButtons mouseButton)
        {
            this.mouseButton = mouseButton;
        }
    }
}

