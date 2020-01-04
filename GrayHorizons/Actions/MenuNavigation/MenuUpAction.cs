namespace GrayHorizons.Actions.MenuNavigation
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.UI;
    using Microsoft.Xna.Framework.Input;

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
            Menu.SelectedIndex = (Menu.SelectedIndex.IsNotNull() ? Menu.SelectedIndex - 1 : Menu.MenuItems.Count - 1);
            base.Execute();
        }
    }
}

