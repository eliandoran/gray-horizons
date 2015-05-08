using System;
using BattleCity.Logic;
using BattleCity.Attributes;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Input.Actions
{
    [DefaultKey (Keys.F12)]
    public class ToggleFullScreenAction: GameAction
    {
        public ToggleFullScreenAction (GameData gameData) : base (gameData) { }

        public override void Execute()
        {
            GameData.GraphicsDeviceManager.ToggleFullScreen ();
        }
    }
}

