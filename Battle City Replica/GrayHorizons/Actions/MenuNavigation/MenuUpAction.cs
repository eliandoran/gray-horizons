using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.UI;

namespace GrayHorizons.Actions.MenuNavigation
{
    [DefaultKey(Keys.Up)]
    public class MenuUp: MenuActionBase
    {
        public MenuUp(
            Menu menu)
            : base(
                menu)
        {
        }

        public override void Execute()
        {
            Menu.SelectedIndex = (Menu.SelectedIndex != null ? Menu.SelectedIndex - 1 : Menu.MenuItems.Count - 1);
            base.Execute();
        }
    }
}

