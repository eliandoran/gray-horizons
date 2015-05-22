/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using System;
using GrayHorizons.Attributes;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Extensions;

namespace GrayHorizons.Actions.PlayerControl
{
    [DefaultKey(Keys.F)]
    public class UseAction: GameAction
    {
        public UseAction(
            Player player)
            : base(
                player)
        {
        }

        public UseAction()
            : this(
                null)
        {

        }

        public override void Execute()
        {
            if (Player.AssignedEntity.IsNotNull())
            {
                Player.AssignedEntity.Use();
            }
            base.Execute();
        }
    }
}

