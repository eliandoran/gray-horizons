using System;
using System.Collections.Generic;
using GrayHorizons.ThirdParty.GameStateManagement;
using Microsoft.Xna.Framework;
using System.Diagnostics;
using GrayHorizons.Extensions;
using GrayHorizons.Input;
using Microsoft.Xna.Framework.Graphics;
using GrayHorizons.Actions.MenuNavigation;

namespace GrayHorizons.UI
{
    public class Menu: GameScreen
    {
        readonly List<MenuItem> menuItems;
        readonly List<InputBinding> inputBindings;
        int? selectedIndex;
        SpriteFont font;

        public Menu ()
        {
            menuItems = new List<MenuItem> ();
            inputBindings = new List<InputBinding> ();

            #if DEBUG
            Debug.WriteLine ("Adding event handlers...", "MENU");
            #endif

            var menuUpAction = new MenuUp (this);

            var menuDownAction = new MenuDown (this);

            var menuSelectAction = new MenuSelect (this);

            var menuMouseControlBinding = new MouseAxisBinding ();
            var menuMouseControlActions = new MenuMouseControlAction (this, menuMouseControlBinding);
            menuMouseControlBinding.BoundAction = menuMouseControlActions;

            inputBindings.AddRange (
                new InputBinding[] {
                    new KeyBinding (menuUpAction),
                    new KeyBinding (menuDownAction),
                    new KeyBinding (menuSelectAction),
                    menuMouseControlBinding
                }
            );

            IsPopup = true;
            Enabled = true;
        }

        public void AddComponents ()
        {
            #if DEBUG
            Debug.WriteLine ("Adding components:", "MENU");
            #endif

            int index = 0;
            foreach (var menuItem in menuItems)
            {
                var pos = new Point (
                              Position.X + index + ItemPadding.X,
                              Position.Y + index * (ItemSize.Y + ItemPadding.Y));
                
                menuItem.Font = font;
                menuItem.Dimensions = new Rectangle (pos.X, pos.Y, ItemSize.X, ItemSize.Y);

                ScreenManager.AddScreen (menuItem, null);

                #if DEBUG
                Debug.WriteLine ("\"{0}\" at {1}.".FormatWith (menuItem.Text, menuItem.Dimensions));
                #endif

                index++;
            }
        }

        public List<MenuItem> MenuItems
        {
            get
            {
                return menuItems;
            }
        }

        public Point Position { get; set; }

        public Point ItemSize { get; set; }

        public Point ItemPadding { get; set; }

        public bool Enabled { get; set; }

        public int Width
        {
            get
            {
                return ItemSize.X - Position.X;
            }
        }

        public int Height
        {
            get
            {
                return (ItemSize.Y + ItemPadding.Y) * MenuItems.Count;
            }
        }

        public int? SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                if (value < 0)
                    selectedIndex = MenuItems.Count - 1;
                else if (value >= MenuItems.Count)
                    selectedIndex = 0;
                else
                    selectedIndex = value;

                UpdateSelection ();
            }
        }

        public MenuItem SelectedMenuItem
        {
            get
            {
                if (SelectedIndex.HasValue)
                {
                    return MenuItems [SelectedIndex.Value];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                if (value != null)
                {
                    SelectedIndex = MenuItems.IndexOf (value);
                }
                else
                    SelectedIndex = null;
                
                UpdateSelection ();
            }
        }

        public MenuItem GetMenuItemAt (
            Point position)
        {
            foreach (MenuItem item in MenuItems)
            {
                if (item.Dimensions.Contains (position))
                    return item;
            }

            return null;
        }

        protected virtual void UpdateSelection ()
        {
            foreach (var menuItem in MenuItems)
            {
                if (menuItem.Selected)
                {
                    menuItem.Selected = false;
                    menuItem.OnDeselect (EventArgs.Empty);
                }
            }

            if (SelectedIndex != null)
            {
                SelectedMenuItem.Selected = true;
                SelectedMenuItem.OnSelect (EventArgs.Empty);
            }
        }

        public override void Update (
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            if (Enabled)
            {
                foreach (InputBinding inputBinding in inputBindings)
                    inputBinding.UpdateState ();
            }

            base.Update (gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void HandleInput (
            InputState input)
        {
            
        }

        public override void LoadContent ()
        {
            font = ScreenManager.Game.Content.Load<SpriteFont> ("Fonts\\Menu");
        }

        public void Unload ()
        {
            foreach (var menuItem in MenuItems)
                menuItem.ExitScreen ();

            ExitScreen ();
        }

        public override void UnloadContent ()
        {
            base.UnloadContent ();
        }

        public override string ToString ()
        {
            return string.Format ("[Menu: ItemsCount={0}]", MenuItems.Count);
        }
    }
}

