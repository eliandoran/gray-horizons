using System;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Attributes
{
    /// <summary>
    /// Indicates the default bound key of an <see cref="BattleCity.Input.InputBinding"/>.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class DefaultKeyAttribute: Attribute
    {
        readonly Keys key;

        /// <summary>
        /// Gets the key that will be bound implicitly to an <see cref="BattleCity.Input.InputBinding"/>.
        /// </summary>
        /// <value>The key.</value>
        public Keys Key
        {
            get
            {
                return key;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Attributes.DefaultKeyAttribute"/> class.
        /// </summary>
        /// <param name="key">The key to bind.</param>
        public DefaultKeyAttribute (Keys key)
        {
            this.key = key;
        }
    }
}
    