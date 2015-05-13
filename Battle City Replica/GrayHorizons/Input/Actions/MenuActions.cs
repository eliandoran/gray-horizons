using System;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Logic;
using System.Diagnostics;
using GrayHorizons.UI;

namespace GrayHorizons.Input.Actions
{
    public static class MenuActions
    {
        public abstract class MenuAction: GameAction
        {
            readonly Menu menu;

            bool PlaySound { get; set; }

            public Menu Menu
            {
                get
                {
                    return menu;
                }
            }

            protected MenuAction (
                Menu menu,
                InputBinding parentInputBinding = null) : base (
                    parentInputBinding: parentInputBinding)
            {
                this.menu = menu;
                PlaySound = true;
            }

            public override void Execute ()
            {
                if (PlaySound)
                    Sound.UISounds.MenuSelect.Play ();

                base.Execute ();
            }
        }


        [DefaultKey (Keys.Up)]
        public class MenuUp: MenuAction
        {
            public MenuUp (
                Menu menu) : base (
                    menu) { }

            public override void Execute ()
            {
                Menu.SelectedIndex = (Menu.SelectedIndex != null ? Menu.SelectedIndex - 1 : Menu.MenuItems.Count - 1);
                base.Execute ();
            }
        }


        [DefaultKey (Keys.Down)]
        public class MenuDown: MenuAction
        {
            public MenuDown (
                Menu menu) : base (
                    menu) { }

            public override void Execute ()
            {
                Menu.SelectedIndex = (Menu.SelectedIndex != null ? Menu.SelectedIndex + 1 : 0);
                base.Execute ();
            }
        }


        [DefaultKey (Keys.Enter)]
        public class MenuSelect: MenuAction
        {
            public MenuSelect (
                Menu menu) : base (
                    menu) { }

            public override void Execute ()
            {
                if (Menu.SelectedMenuItem != null)
                    Menu.SelectedMenuItem.Execute ();
                
                base.Execute ();
            }
        }


        public class MenuMouseControlAction: MenuAction
        {
            MenuItem lastMenuItem = null;

            public MenuMouseControlAction (
                Menu menu,
                MouseAxisBinding parentInputBinding) : base (
                    menu,
                    parentInputBinding: parentInputBinding)
            {
                ParentInputBindingChanged += MenuMouseBinding_ParentInputBindingChanged;
                OnParentInputBindingChanged (EventArgs.Empty);
            }

            void MenuMouseBinding_ParentInputBindingChanged (
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

            void MouseAxisBinding_MouseStateChanged (
                object sender,
                MouseStateChangedEventArgs e)
            {
                if (e.State.LeftButton == ButtonState.Pressed)
                {
                    var item = Menu.GetMenuItemAt (e.State.Position);
                    if (item != null)
                    {
                        item.Execute ();
                        base.Execute ();
                    }
                }
            }

            void MouseAxisBinding_AxisChanged (
                object sender,
                AxisChangedEventArgs e)
            {
                var item = Menu.GetMenuItemAt (e.State.Position);
                Menu.SelectedMenuItem = item;

                if (item != null && lastMenuItem != item)
                {
                    Sound.UISounds.MenuSelect.Play ();
                }

                lastMenuItem = Menu.SelectedMenuItem;
            }
        }
    }
}

