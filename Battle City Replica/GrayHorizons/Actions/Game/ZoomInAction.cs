namespace GrayHorizons.Actions.Game
{
    using System;
    using System.Diagnostics;
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.OemPlus)]
    [AllowContinuousPress]
    public class ZoomInAction: GameAction
    {
        public override void Execute()
        {
            if (GameData.MapScale.X < 2.5f)
            {
                const float step = 0.01f;
                var mapScale = GameData.MapScale.X + step;
                var viewportScale = GameData.ViewportScale.X - step;
                GameData.MapScale = new Vector2(mapScale, mapScale);
                GameData.ViewportScale = new Vector2(viewportScale, viewportScale);
                GameData.Map.ScaledViewport = GameData.Map.Viewport.ScaleTo(GameData.ViewportScale);
                GameData.Map.CenterViewportAt(GameData.ActivePlayer.AssignedEntity);
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

