namespace GrayHorizons.Events
{
    using System;
    using GrayHorizons.Extensions;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class AxisChangedEventArgs: EventArgs
    {
        public MouseState State { get; set; }

        public Point Delta { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.AxisChangedEventArgs"/> class.
        /// </summary>
        public AxisChangedEventArgs()
        {
        }

        public AxisChangedEventArgs(MouseState state, Point delta)
        {
            State = state;
            Delta = delta;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.AxisChangedEventArgs"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.AxisChangedEventArgs"/>.</returns>
        public override string ToString()
        {
            return "[AxisChangedEventArgs: Position={0}, Delta={1}]".FormatWith(State.Position, Delta);
        }
    }
}

