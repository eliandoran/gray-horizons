namespace GrayHorizons.Input
{
    using System;
    using GrayHorizons.Events;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    public class MouseAxisBinding: AxisBinding
    {
        public event EventHandler<MouseStateChangedEventArgs> MouseStateChanged;

        int lastX = -1,
            lastY = -1;

        MouseState lastState;

        public MouseAxisBinding(
            GameData gameData,
            GameAction boundAction = null)
            : base(
                gameData,
                boundAction)
        {
        }

        public MouseAxisBinding()
            : this(
                null)
        {
        }

        public override void UpdateState()
        {
            MouseState state = Mouse.GetState();

            if (!IsActive())
                return;

            if (state.X != lastX ||
                state.Y != lastY)
            {
                var delta = new Point(state.X - lastX, state.Y - lastY);
                lastX = state.X;
                lastY = state.Y;
                OnAxisChanged(new AxisChangedEventArgs(state, delta));
            }

            if (state != lastState)
            {
                lastState = state;
                OnStateChanged(new MouseStateChangedEventArgs(state));
            }
        }

        protected void OnStateChanged(
            MouseStateChangedEventArgs eventArgs)
        {
            if (!IsActive())
                return;

            if (MouseStateChanged.IsNotNull())
                MouseStateChanged(this, eventArgs);
        }

        public override bool IsActive()
        {
            return Game.IsNotNull() && Game.IsActive;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.MouseAxisBinding"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.MouseAxisBinding"/>.</returns>
        public override string ToString()
        {
            return "[MouseAxisBinding, lastX={0}, lastY={1}]".FormatWith(
                lastX, lastY);
        }
    }
}

