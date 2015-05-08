using System;
using BattleCity.Input;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Logic
{
    public abstract class GameAction
    {
        public Player Player { get; set; }

        public GameData GameData { get; set; }

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

        public abstract void Execute();
    }
}

