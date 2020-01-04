namespace GrayHorizons.Events
{
    using System;
    using Microsoft.Xna.Framework.Input;

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
}

