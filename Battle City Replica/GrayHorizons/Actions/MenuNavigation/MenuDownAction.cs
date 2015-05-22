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
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using GrayHorizons.Extensions;

namespace GrayHorizons.Actions.MenuNavigation
{
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

