namespace GrayHorizons.Attributes
{
    using System;
    using GrayHorizons.Logic;

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

