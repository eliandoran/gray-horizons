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
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.UI;
using GrayHorizons.Extensions;

namespace GrayHorizons.Actions.MenuNavigation
{
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

