using System;
using BattleCity.Logic;

namespace BattleCity.Attributes
{
    public class DefaultMouseButtonAttribute: Attribute
    {
        readonly MouseButtons mouseButton;

        /// <summary>
        /// Gets the key that will be bound implicitly to an <see cref="BattleCity.Input.InputBinding"/>.
        /// </summary>
        /// <value>The key.</value>
        public MouseButtons MouseButtons
        {
            get
            {
                return mouseButton;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Attributes.DefaultKeyAttribute"/> class.
        /// </summary>
        /// <param name="key">The key to bind.</param>
        public DefaultMouseButtonAttribute (
            MouseButtons mouseButton)
        {
            this.mouseButton = mouseButton;
        }
    }
}

