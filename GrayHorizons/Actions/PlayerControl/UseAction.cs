namespace GrayHorizons.Actions.PlayerControl
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.Logic;
    using Microsoft.Xna.Framework.Input;

    [DefaultKey(Keys.F)]
    public class UseAction: GameAction
    {
        public UseAction(
            Player player)
            : base(
                player)
        {
        }

        public UseAction()
            : this(
                null)
        {

        }

        public override void Execute()
        {
            if (Player.AssignedEntity.IsNotNull())
            {
                Player.AssignedEntity.Use();
            }
            base.Execute();
        }
    }
}

