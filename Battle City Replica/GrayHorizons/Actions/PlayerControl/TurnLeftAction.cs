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

