using System.Xml.Serialization;
using GrayHorizons.Logic;
using GrayHorizons.StaticObjects;
using GrayHorizons.ThirdParty;

namespace GrayHorizons.Logic
{
    /// <summary>
    /// Represents an immovable object on the in-game map.
    /// </summary>
    [XmlInclude (typeof(Wall))]
    public abstract class StaticObject: ObjectBase
    {
        public override void Render ()
        {
            if (Position.Intersects (new RotatedRectangle (GameData.Map.Viewport, 0)))
            {
                var texture = GameData.MappedTextures [GetType ()];
                var viewportPosition = GameData.Map.CalculateViewportCoordinates (Position.UpperLeftCorner (),
                                                                                  GameData.MapScale);

                GameData.SpriteBatch.Draw (texture, viewportPosition, scale: GameData.MapScale);
            }
        }
    }
}

