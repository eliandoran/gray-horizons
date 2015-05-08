using System.Xml.Serialization;
using BattleCity.Logic;
using BattleCity.StaticObjects;

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
    }
}

