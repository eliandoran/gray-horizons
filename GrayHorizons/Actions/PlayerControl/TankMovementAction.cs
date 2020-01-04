namespace GrayHorizons.Actions.PlayerControl
{
    using System;
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.Input;
    using GrayHorizons.Logic;

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

