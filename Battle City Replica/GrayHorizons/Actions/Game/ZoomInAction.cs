using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.Game
{
    [DefaultKey(Keys.OemPlus)]
    [AllowContinuousPress]
    public class ZoomInAction: GameAction
    {
        public ZoomInAction(
            GameData gameData)
            : base(
                gameData)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ZoomInAction"/> class.
        /// </summary>
        public ZoomInAction()
            : this(
                null)
        {
        }

        public override void Execute()
        {
            if (GameData.MapScale.X < 2.0f)
            {
                const float step = 0.01f;
                var mapScale = GameData.MapScale.X + step;
                var viewportScale = GameData.ViewportScale.X - step;
                GameData.MapScale = new Vector2(mapScale, mapScale);
                GameData.Map.ScaledViewport = GameData.Map.Viewport.ScaleTo(GameData.ViewportScale.X);
                GameData.ViewportScale = new Vector2(viewportScale, viewportScale);
            }
            #if DEBUG
            else
            {
                Debug.WriteLine("Maximum zoom reached.", "ZOOM");
            }
            #endif
        }
    }
}

