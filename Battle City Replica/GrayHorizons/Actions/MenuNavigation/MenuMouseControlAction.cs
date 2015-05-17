using System;
using GrayHorizons.UI;
using GrayHorizons.Input;
using Microsoft.Xna.Framework.Input;

namespace GrayHorizons.Actions.MenuNavigation
{
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
            if (mouseAxisBinding != null)
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
                if (item != null)
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

            if (item != null && lastMenuItem != item)
            {
                Sound.UISounds.MenuSelect.Play();
            }

            lastMenuItem = Menu.SelectedMenuItem;
        }
    }
}

