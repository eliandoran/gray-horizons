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
using GrayHorizons.Input;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.PlayerControl
{
    [DefaultKey(Keys.A)]
    public class TurnLeftAction: TankMovementAction
    {
        public TurnLeftAction(
            Player player,
            InputBinding parentInputBinding)
            : base(
                player,
                parentInputBinding,
                MovementType.Turning)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.TurnLeftAction"/> class.
        /// </summary>
        public TurnLeftAction()
            : this(
                null,
                null)
        {
        }

        public override void Execute()
        {
            Player.AssignedEntity.Turn(Entity.TurnDirection.Left);
        }
    }
}

