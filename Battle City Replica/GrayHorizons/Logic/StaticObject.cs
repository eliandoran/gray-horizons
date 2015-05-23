namespace GrayHorizons.Logic
{
    using System.Xml.Serialization;
    using GrayHorizons.Extensions;
    using GrayHorizons.StaticObjects;
    using GrayHorizons.ThirdParty;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Represents an immovable object on the in-game map.
    /// </summary>
    [XmlInclude(typeof(Wall))]
    public abstract class StaticObject: ObjectBase
    {
        protected StaticObject()
        {
            MiniMapColor = Color.Gray;
        }

        public override void Render()
        {
            if (Position.Intersects(new RotatedRectangle(GameData.Map.ScaledViewport, 0)))
            {
                Texture2D texture;
                if (CustomTexture.IsNotNull())
                    texture = CustomTexture;
                else if (GameData.MappedTextures.ContainsKey(GetType()))
                    texture = GameData.MappedTextures[GetType()];
                else
                    return;

                var viewportPosition = GameData.Map.CalculateViewportCoordinates(
                                           Position.UpperLeftCorner(),
                                           GameData.MapScale);
                

                GameData.ScreenManager.SpriteBatch.Draw(texture,
                    viewportPosition,
                    scale: GameData.MapScale,
                    sourceRectangle: CustomTextureCrop,
                    rotation: Position.Rotation);
            }
        }

        public override void RenderHud()
        {
            // No implementation needed.
        }
    }
}

