using System;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Xml.Serialization;
using GrayHorizons.Logic;

namespace GrayHorizons.Input
{
    public class AxisChangedEventArgs: EventArgs
    {
        public MouseState State { get; set; }

        public Point Delta { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.AxisChangedEventArgs"/> class.
        /// </summary>
        public AxisChangedEventArgs () { }

        public AxisChangedEventArgs (
            MouseState state,
            Point delta)
        {
            State = state;
            Delta = delta;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.AxisChangedEventArgs"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.AxisChangedEventArgs"/>.</returns>
        public override string ToString ()
        {
            return string.Format ("[AxisChangedEventArgs: Position={0}, Delta={1}]", State.Position, Delta);
        }
    }


    [XmlInclude (typeof(MouseAxisBinding))]
    public abstract class AxisBinding: InputBinding
    {
        /// <summary>
        /// Occurs when the axis changes its X or Y position.
        /// </summary>
        public event EventHandler<AxisChangedEventArgs> AxisChanged;

        protected AxisBinding (
            GameData gameData,
            GameAction boundAction = null) : base (
                gameData,
                boundAction,
                true) { }

        /// <summary>
        /// Raises the axis changed event.
        /// </summary>
        /// <param name="e">The <see cref="GrayHorizons.Input.AxisChangedEventArgs"/> containing information for this event.</param>
        protected void OnAxisChanged (
            AxisChangedEventArgs e)
        {
            if (AxisChanged != null)
                AxisChanged (this, e);
        }
    }
}

