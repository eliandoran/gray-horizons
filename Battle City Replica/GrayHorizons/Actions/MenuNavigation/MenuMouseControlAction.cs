/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using System;
using GrayHorizons.UI;
using GrayHorizons.Input;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Extensions;

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

