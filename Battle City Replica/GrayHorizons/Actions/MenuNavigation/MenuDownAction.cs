namespace GrayHorizons.Actions.MenuNavigation
{
    using GrayHorizons.Attributes;
    using GrayHorizons.Extensions;
    using GrayHorizons.UI;
    using Microsoft.Xna.Framework.Input;

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
            Menu.SelectedIndex = (Menu.SelectedIndex.IsNotNull() ? Menu.SelectedIndex + 1 : 0);
            base.Execute();
        }
    }
}

