namespace GrayHorizons.Actions.PlayerControl
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Input;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

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

