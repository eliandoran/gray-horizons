using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;
using GrayHorizons.Extensions;
using System.Diagnostics;

namespace GrayHorizons.Input.Actions
{
    [DefaultKey (Keys.OemPlus)]
    [AllowContinuousPress]
    public class ZoomInAction: GameAction
    {
        public ZoomInAction (
            GameData gameData) : base (
                gameData) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ZoomInAction"/> class.
        /// </summary>
        public ZoomInAction () : this (
                null) { }

        public override void Execute ()
        {
            if (GameData.MapScale.X < 2.0f)
            {
                const float step = 0.01f;
                var mapScale = GameData.MapScale.X + step;
                var viewportScale = GameData.ViewportScale.X - step;
                GameData.MapScale = new Vector2 (mapScale, mapScale);
                GameData.Map.ScaledViewport = GameData.Map.Viewport.ScaleTo (GameData.ViewportScale.X);
                GameData.ViewportScale = new Vector2 (viewportScale, viewportScale);
            }
            #if DEBUG
            else
            {
                Debug.WriteLine ("Maximum zoom reached.", "ZOOM");
            }
            #endif
        }
    }


    [DefaultKey (Keys.OemMinus)]
    [AllowContinuousPress]
    public class ZoomOutAction: GameAction
    {
        public ZoomOutAction (
            GameData gameData) : base (
                gameData) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ZoomOutAction"/> class.
        /// </summary>
        public ZoomOutAction () : this (
                null) { }

        public override void Execute ()
        {
            if (GameData.MapScale.X > 0.1f)
            {
                const float step = 0.01f;
                //var scale = GameData.Scale.X - step;

                var viewportScale = GameData.ViewportScale.X + step;
                var mapScale = GameData.MapScale.X - step;

                var newViewport = GameData.Map.Viewport.ScaleTo (viewportScale);
                Debug.WriteLine ("New viewport: {0}, map: {1}", viewportScale, GameData.Map.MapSize);

                if (ViewportFits (newViewport))
                {
                    GameData.MapScale = new Vector2 (mapScale, mapScale);
                    GameData.ViewportScale = new Vector2 (viewportScale, viewportScale);
                    GameData.Map.ScaledViewport = newViewport;
                }
                #if DEBUG
                else
                {
                    Debug.WriteLine ("Scaled viewport no longer fits the map size. Aborted.", "ZOOM");
                }
                #endif
            }
        }

        bool ViewportFits (Rectangle viewport)
        {
            Debug.WriteLine (viewport);

            return !(
                (viewport.Right > GameData.Map.MapSize.X) ||
                (viewport.Bottom > GameData.Map.MapSize.Y) ||
                (viewport.Left < 0) || (viewport.Top < 0)
            );
        }
    }
}

