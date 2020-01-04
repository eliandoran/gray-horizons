namespace GrayHorizons.Actions.MenuNavigation
{
    using System;
    using GrayHorizons.Events;
    using GrayHorizons.Extensions;
    using GrayHorizons.Input;
    using GrayHorizons.UI;
    using Microsoft.Xna.Framework.Input;

    public class MenuMouseControlAction: MenuActionBase
    {
        MenuItem lastMenuItem = null;

        public MenuMouseControlAction(
            Menu menu,
            MouseAxisBinding parentInputBinding)
            : base(
                menu,
                parentInputBinding: parentInputBinding)
        {
            ParentInputBindingChanged += MenuMouseBinding_ParentInputBindingChanged;
            OnParentInputBindingChanged(EventArgs.Empty);
        }

        void MenuMouseBinding_ParentInputBindingChanged(
            object sender,
            EventArgs e)
        {
            var mouseAxisBinding = ParentInputBinding as MouseAxisBinding;
            if (mouseAxisBinding.IsNotNull())
            {
                mouseAxisBinding.AxisChanged += MouseAxisBinding_AxisChanged;
                mouseAxisBinding.MouseStateChanged += MouseAxisBinding_MouseStateChanged;
            }
        }

        void MouseAxisBinding_MouseStateChanged(
            object sender,
            MouseStateChangedEventArgs e)
        {
            if (e.State.LeftButton == ButtonState.Pressed)
            {
                var item = Menu.GetMenuItemAt(e.State.Position);
                if (item.IsNotNull())
                {
                    item.Execute();
                    base.Execute();
                }
            }
        }

        void MouseAxisBinding_AxisChanged(
            object sender,
            AxisChangedEventArgs e)
        {
            var item = Menu.GetMenuItemAt(e.State.Position);
            Menu.SelectedMenuItem = item;

            if (item.IsNotNull() && lastMenuItem != item)
            {
                Sound.UISounds.MenuSelect.Play();
            }

            lastMenuItem = Menu.SelectedMenuItem;
        }
    }
}

