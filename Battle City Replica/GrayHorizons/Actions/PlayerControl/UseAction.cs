using System;
using GrayHorizons.Attributes;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework.Input;

namespace GrayHorizons.Actions.PlayerControl
{
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
            if (Player.AssignedEntity != null)
            {
                Player.AssignedEntity.Use();
            }
            base.Execute();
        }
    }
}

