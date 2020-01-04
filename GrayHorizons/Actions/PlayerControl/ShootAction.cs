namespace GrayHorizons.Actions.PlayerControl
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.Space)]
    [DefaultMouseButton(MouseButtons.Left)]
    [AllowContinuousPress]
    public class ShootAction: GameAction
    {
        public ShootAction(
            Player player)
            : base(
                player)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Input.Actions.ShootAction"/> class.
        /// </summary>
        public ShootAction()
            : this(
                null)
        {
        }

        public override void Execute()
        {
            Player.AssignedEntity.Shoot();
        }
    }
}

