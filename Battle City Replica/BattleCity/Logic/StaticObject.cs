using System.Xml.Serialization;
using BattleCity.Logic;
using BattleCity.StaticObjects;
using BattleCity.ThirdParty;

namespace BattleCity.Logic
{
    /// <summary>
    /// Represents an immovable object on the in-game map.
    /// </summary>
    [XmlInclude (typeof(Wall))]
    public abstract class StaticObject: ObjectBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.StaticObject"/> class.
        /// </summary>
        public StaticObject ()
        {

        }

        public override void Render()
        {
            if (Position.Intersects (new RotatedRectangle (GameData.Map.Viewport, 0)))
            {
                var texture = GameData.MappedTextures [GetType ()];
                var viewportPosition = GameData.Map.CalculateViewportCoordinates (Position.UpperLeftCorner (),
                                                                                  GameData.Scale);

                GameData.SpriteBatch.Draw (texture, viewportPosition, scale: GameData.Scale);
            }
        }
    }
}

