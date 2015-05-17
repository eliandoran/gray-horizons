using System;
using GrayHorizons.UI;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;

namespace GrayHorizons.Actions.MenuNavigation
{
    [DefaultKey(Keys.Down)]
    public class MenuDown: MenuActionBase
    {
        public MenuDown(
            Menu menu)
            : base(
                menu)
        {
        }

        public override void Execute()
        {
            Menu.SelectedIndex = (Menu.SelectedIndex != null ? Menu.SelectedIndex + 1 : 0);
            base.Execute();
        }
    }
}

