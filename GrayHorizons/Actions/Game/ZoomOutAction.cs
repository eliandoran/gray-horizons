﻿namespace GrayHorizons.Actions.Game
{
    using System;
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.OemMinus)]
    [AllowContinuousPress]
    public class ZoomOutAction: GameAction
    {
        public override void Execute()
        {
            if (GameData.MapScale.X > 0.1f)
            {
                const float step = 0.01f;
                var mapScale = GameData.MapScale.X - step;
                var viewportScale = GameData.ViewportScale.X + step;
                GameData.MapScale = new Vector2(mapScale, mapScale);
                GameData.Map.ScaledViewport = GameData.Map.Viewport.ScaleTo(GameData.ViewportScale);
                GameData.ViewportScale = new Vector2(viewportScale, viewportScale);
                GameData.Map.CenterViewportAt(GameData.ActivePlayer.AssignedEntity);
                //var newViewport = GameData.Map.Viewport.ScaleTo(GameData.MapScale);
            }
        }

        bool ViewportFits(Rectangle viewport)
        {
            return !(
                (viewport.Right > GameData.Map.MapSize.X) ||
                (viewport.Bottom > GameData.Map.MapSize.Y) ||
                (viewport.Left < 0) || (viewport.Top < 0)
            );
        }
    }
}

