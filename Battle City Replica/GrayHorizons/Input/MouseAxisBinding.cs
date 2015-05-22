using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using GrayHorizons.Logic;
using System.Diagnostics;
using GrayHorizons.Extensions;

namespace GrayHorizons.Input
{
    public class MouseStateChangedEventArgs: EventArgs
    {
        MouseState state;

        public MouseState State
        {
            get
            {
                return state;
            }
        }

        public MouseStateChangedEventArgs(
            MouseState state = new MouseState())
        {
            this.state = state;
        }
    }


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
            return string.Format("[MouseAxisBinding, lastX={0}, lastY={1}]", lastX, lastY);
        }
    }
}

