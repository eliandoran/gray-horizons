namespace GrayHorizons.Actions.MenuNavigation
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.UI;
    using Microsoft.Xna.Framework.Input;

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
            if (Menu.SelectedMenuItem.IsNotNull())
                Menu.SelectedMenuItem.Execute();

            base.Execute();
        }
    }
}

