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
using GrayHorizons.Logic;
using GrayHorizons.Extensions;

namespace GrayHorizons.Actions.PlayerControl
{
    [AllowContinuousPress()]
    public abstract class TankMovementAction: GameAction
    {
        /// <summary>
        /// Represents whether a tank is moving (forward or backward) or turning (left or right).
        /// </summary>
        protected enum MovementType
        {
            Moving,
            Turning
        }

        MovementType movementType;

        protected TankMovementAction(
            Player player,
            InputBinding parentInputBinding,
            MovementType movementType)
            : base(
                null,
                player,
                parentInputBinding)
        {
            this.movementType = movementType;

            var keyBinding = ParentInputBinding as KeyBinding;
            if (keyBinding.IsNotNull())
            {
                keyBinding.KeyDown += ParentKeyBinding_KeyDown;
                keyBinding.KeyUp += ParentKeyBinding_KeyUp;
            }
        }

        void ParentKeyBinding_KeyUp(
            object sender,
            EventArgs e)
        {
            SetValue(false);
        }

        void ParentKeyBinding_KeyDown(
            object sender,
            EventArgs e)
        {
            SetValue(true);
        }

        void SetValue(
            bool value)
        {
            switch (movementType)
            {
                case MovementType.Moving:
                    Player.AssignedEntity.IsMoving = value;
                    break;

                case MovementType.Turning:
                    Player.AssignedEntity.IsTurning = value;
                    break;
            }
        }
    }
}

