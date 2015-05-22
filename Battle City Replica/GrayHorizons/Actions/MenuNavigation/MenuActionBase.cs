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
using GrayHorizons.Logic;

namespace GrayHorizons.Actions.MenuNavigation
{
    public abstract class MenuActionBase: GameAction
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

        protected MenuActionBase(
            Menu menu,
            InputBinding parentInputBinding = null)
            : base(
                parentInputBinding: parentInputBinding)
        {
            this.menu = menu;
            PlaySound = true;
        }

        public override void Execute()
        {
            if (PlaySound)
                Sound.UISounds.MenuSelect.Play();

            base.Execute();
        }
    }
}