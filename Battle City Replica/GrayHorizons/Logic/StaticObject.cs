using System.Xml.Serialization;
using GrayHorizons.StaticObjects;
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Logic
{
    /// <summary>
    /// Represents an immovable object on the in-game map.
    /// </summary>
    [XmlInclude (typeof(Wall))]
    public abstract class StaticObject: ObjectBase
    {
        protected StaticObject ()
        {
            MinimapColor = Color.Gray;
        }

        public override void Render ()
        {
            if (Position.Intersects (new RotatedRectangle (GameData.Map.Viewport, 0)))
            {
                var texture = CustomTexture ?? GameData.MappedTextures [GetType ()];
                var viewportPosition = GameData.Map.CalculateViewportCoordinates (Position.UpperLeftCorner (),
                                                                                  GameData.MapScale);
                

                GameData.ScreenManager.SpriteBatch.Draw (texture,
                                                         viewportPosition,
                                                         scale: GameData.MapScale,
                                                         sourceRectangle: CustomTextureCrop,
                                                         rotation: Position.Rotation);
            }
        }

        public override void RenderHUD ()
        {
            // No implementation needed.
        }
    }
}

