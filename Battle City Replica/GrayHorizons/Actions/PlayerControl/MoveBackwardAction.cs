using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Input;
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.PlayerControl
{
    [DefaultKey(Keys.S)]
    public class MoveBackwardAction: TankMovementAction
    {
        public MoveBackwardAction(
            Player player,
            InputBinding parentInputBinding)
            : base(
                player,
                parentInputBinding,
                MovementType.Moving)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.MoveBackwardAction"/> class.
        /// </summary>
        public MoveBackwardAction()
            : this(
                null,
                null)
        {
        }

        public override void Execute()
        {
            Player.AssignedEntity.IsMoving = true;
            Player.AssignedEntity.Move(Entity.MoveDirection.Backward, false);
        }
    }
}

