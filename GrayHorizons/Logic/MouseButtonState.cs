﻿namespace GrayHorizons.Logic
{
    using Microsoft.Xna.Framework.Input;

    public enum MouseButtons
    {
        Left,
        Right,
        Middle,
        X1,
        X2
    }


    public class MouseButtonState
    {
        ButtonState LeftButton { get; set; }

        ButtonState RightButton { get; set; }

        ButtonState MiddleButton { get; set; }

        ButtonState XButton1 { get; set; }

        ButtonState XButton2 { get; set; }

        public MouseButtonState()
        {
            
        }

        public MouseButtonState(
            MouseState state)
        {
            LeftButton = state.LeftButton;
            RightButton = state.RightButton;
            MiddleButton = state.MiddleButton;
            XButton1 = state.XButton1;
            XButton2 = state.XButton2;
        }
    }
}

