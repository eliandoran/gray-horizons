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

