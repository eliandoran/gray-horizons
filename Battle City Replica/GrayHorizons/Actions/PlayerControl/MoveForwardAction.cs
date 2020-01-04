﻿/*
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
using GrayHorizons.Input;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.PlayerControl
{
    [DefaultKey(Keys.W)]
    public class MoveForwardAction: TankMovementAction
    {
        public MoveForwardAction(
            Player player,
            InputBinding parentInputBinding)
            : base(
                player,
                parentInputBinding,
                MovementType.Moving)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.MoveForwardAction"/> class.
        /// </summary>
        public MoveForwardAction()
            : this(
                null,
                null)
        {
        }

        public override void Execute()
        {
            Player.AssignedEntity.IsMoving = true;
            Player.AssignedEntity.Move(Entity.MoveDirection.Forward, false);
        }
    }
}

