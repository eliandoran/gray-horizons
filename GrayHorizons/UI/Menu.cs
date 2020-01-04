namespace GrayHorizons.UI
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using GrayHorizons.Actions.MenuNavigation;
    using GrayHorizons.Extensions;
    using GrayHorizons.Input;
    using GameStateManagement;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Menu: GameScreen
    {
        readonly List<MenuItem> menuItems = new List<MenuItem>();
        readonly List<InputBinding> inputBindings = new List<InputBinding>();
        int? selectedIndex;
        SpriteFont font;

        public Menu()
        {
            Debug.WriteLine("Adding event handlers...", "MENU");

            var menuUpAction = new MenuUp(this);
            var menuDownAction = new MenuDown(this);
            var menuSelectAction = new MenuSelect(this);
            var menuMouseControlBinding = new MouseAxisBinding();
            var menuMouseControlActions = new MenuMouseControlAction(this, menuMouseControlBinding);
            menuMouseControlBinding.BoundAction = menuMouseControlActions;

            inputBindings.AddRange(
                new InputBinding[]
                {
                    new KeyBinding(menuUpAction),
                    new KeyBinding(menuDownAction),
                    new KeyBinding(menuSelectAction),
                    menuMouseControlBinding
                }
            );

            IsPopup = true;
            Enabled = true;
        }

        public void RepositionComponents()
        {
            int index = 0;
            foreach (var menuItem in menuItems)
            {
                var pos = new Point(
                              Position.X + ItemPadding.X,
                              Position.Y + index * (ItemSize.Y + ItemPadding.Y));

                menuItem.Font = font;
                menuItem.Dimensions = new Rectangle(pos.X, pos.Y, ItemSize.X, ItemSize.Y);

                Debug.WriteLine("\"{0}\" at {1}.".FormatWith(menuItem.Text, menuItem.Dimensions));

                index++;
            }
        }

        public void AddComponents()
        {
            Debug.WriteLine("Adding components:", "MENU");

            RepositionComponents();
            foreach (var menuItem in menuItems) { 
                ScreenManager.AddScreen(menuItem, null);
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

                UpdateSelection();
            }
        }

        public MenuItem SelectedMenuItem
        {
            get
            {
                return SelectedIndex.HasValue ? MenuItems[SelectedIndex.Value] : null;
            }

            set
            {
                SelectedIndex = value.IsNotNull() ? (int?)MenuItems.IndexOf(value) : null;                
                UpdateSelection();
            }
        }

        public MenuItem GetMenuItemAt(
            Point position)
        {
            return MenuItems.Find(item => item.Dimensions.Contains(position));
        }

        protected virtual void UpdateSelection()
        {
            MenuItems.FindAll(item => item.Selected).ForEach(item =>
                {
                    item.Selected = false;
                    item.OnSelect(EventArgs.Empty); 
                });

            if (SelectedIndex.IsNotNull())
            {
                SelectedMenuItem.Selected = true;
                SelectedMenuItem.OnSelect(EventArgs.Empty);
            }
        }

        public override void Update(
            GameTime gameTime,
            bool otherScreenHasFocus,
            bool coveredByOtherScreen)
        {
            if (Enabled)
                inputBindings.ForEach(binding => binding.UpdateState());

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Activate(bool instancePreserved)
        {
            font = ScreenManager.Game.Content.Load<SpriteFont>("Fonts\\Menu");
            inputBindings.ForEach(binding => binding.Game = ScreenManager.Game);
        }

        public override void Unload()
        {
            MenuItems.ForEach(item => item.ExitScreen());
        }

        public override string ToString()
        {
            return "[Menu: ItemsCount={0}]".FormatWith(MenuItems.Count);           
        }
    }
}

