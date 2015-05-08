using System;
using BattleCity.Logic;

namespace BattleCity.Input
{
    public abstract class InputBinding
    {
        readonly GameData gameData;
        GameAction boundAction;

        public GameAction BoundAction
        {
            get
            {
                return boundAction;
            }
            set
            {
                boundAction = value;

                if (boundAction != null)
                    boundAction.ParentInputBinding = this;
            }
        }

        public Player Player { get; set; }

        public bool AllowContinousPress { get; set; }

        GameData GameData
        {
            get
            {
                return gameData;
            }
        }

        internal InputBinding (GameData gameData,
                               GameAction boundAction,
                               Player player,
                               bool allowContinousPress)
        {
            this.gameData = gameData;
            AllowContinousPress = allowContinousPress;
            BoundAction = boundAction;
            Player = player;
        }

        internal InputBinding (GameData gameData,
                               GameAction boundAction,
                               bool allowContinousPress) : this (gameData,
                                                                 boundAction,
                                                                 null,
                                                                 allowContinousPress)
        {
            
        }

        internal InputBinding (GameData gameData) : this (gameData,
                                                          null,
                                                          false)
        {

        }

        public abstract void UpdateState();

        public abstract bool IsActive();

        public void RunIfActive()
        {
            if (IsActive ())
                BoundAction.Execute ();
        }
    }
}

