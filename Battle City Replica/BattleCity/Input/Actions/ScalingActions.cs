using System;
using BattleCity.Attributes;
using Microsoft.Xna.Framework.Input;
using BattleCity.Logic;
using Microsoft.Xna.Framework;
using BattleCity.Extensions;

namespace BattleCity.Input.Actions
{
    [DefaultKey (Keys.OemPlus)]
    [AllowContinousPress]
    public class ZoomInAction: GameAction
    {
        public ZoomInAction (
            GameData gameData) : base (
                gameData) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Input.Actions.ZoomInAction"/> class.
        /// </summary>
        public ZoomInAction () : this (
                null) { }

        public override void Execute ()
        {
            if (GameData.Scale.X < 2.0f)
            {
                var scale = GameData.Scale.X + 0.01f;
                GameData.Scale = new Vector2 (scale, scale);
                GameData.Map.ScaledViewport = GameData.Map.Viewport.Scale (scale);
            }
        }
    }


    [DefaultKey (Keys.OemMinus)]
    [AllowContinousPress]
    public class ZoomOutAction: GameAction
    {
        public ZoomOutAction (
            GameData gameData) : base (
                gameData) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Input.Actions.ZoomOutAction"/> class.
        /// </summary>
        public ZoomOutAction () : this (
                null) { }

        public override void Execute ()
        {
            if (GameData.Scale.X > 0.1f)
            {
                var scale = GameData.Scale.X - 0.01f;
                GameData.Scale = new Vector2 (scale, scale);
                GameData.Map.ScaledViewport = GameData.Map.Viewport.Scale (scale);
            }
        }
    }
}

