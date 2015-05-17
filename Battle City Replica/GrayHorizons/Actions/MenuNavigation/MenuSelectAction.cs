using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.UI;

namespace GrayHorizons.Actions.MenuNavigation
{
    [DefaultKey(Keys.Enter)]
    public class MenuSelect: MenuActionBase
    {
        public MenuSelect(
            Menu menu)
            : base(
                menu)
        {
        }

        public override void Execute()
        {
            if (Menu.SelectedMenuItem != null)
                Menu.SelectedMenuItem.Execute();

            base.Execute();
        }
    }
}

