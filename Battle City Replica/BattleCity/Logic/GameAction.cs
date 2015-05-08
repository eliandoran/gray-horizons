using System;
using BattleCity.Input;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

namespace BattleCity.Logic
{
    public abstract class GameAction
    {
        [XmlIgnore ()]
        public Player Player { get; set; }

        [XmlIgnore ()]
        public GameData GameData { get; set; }

        [XmlIgnore ()]
        public InputBinding ParentInputBinding { get; set; }

        internal GameAction (GameData gameData = null,
                             Player player = null,
                             InputBinding parentInputBinding = null)
        {
            GameData = gameData;
            Player = player;
            ParentInputBinding = parentInputBinding;
        }

        internal GameAction (Player player) : this (null,
                                                    player,
                                                    null)
        {
            
        }

        internal GameAction () : this (null)
        {
            
        }

        public abstract void Execute();
    }
}

