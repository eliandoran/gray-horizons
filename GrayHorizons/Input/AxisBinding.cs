namespace GrayHorizons.Input
{
    using System;
    using System.Xml.Serialization;
    using GrayHorizons.Events;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;

    [XmlInclude(typeof(MouseAxisBinding))]
    public abstract class AxisBinding: InputBinding
    {
        /// <summary>
        /// Occurs when the axis changes its X or Y position.
        /// </summary>
        public event EventHandler<AxisChangedEventArgs> AxisChanged;

        protected AxisBinding(GameData gameData, GameAction boundAction = null)
            : base(gameData, boundAction, true)
        {
        }

        /// <summary>
        /// Raises the axis changed event.
        /// </summary>
        /// <param name="e">The <see cref="GrayHorizons.Input.AxisChangedEventArgs"/> containing information for this event.</param>
        protected void OnAxisChanged(
            AxisChangedEventArgs e)
        {
            if (AxisChanged.IsNotNull())
                AxisChanged(this, e);
        }
    }
}

