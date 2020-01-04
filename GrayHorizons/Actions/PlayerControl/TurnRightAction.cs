namespace GrayHorizons.Actions.PlayerControl
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Input;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.D)]
    public class TurnRightAction: TankMovementAction
    {
        public TurnRightAction(
            Player player,
            InputBinding parentInputBinding)
            : base(
                player,
                parentInputBinding,
                MovementType.Turning)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.TurnRightAction"/> class.
        /// </summary>
        public TurnRightAction()
            : this(
                null,
                null)
        {
        }

        public override void Execute()
        {
            Player.AssignedEntity.Turn(Entity.TurnDirection.Right);
        }
    }
}

