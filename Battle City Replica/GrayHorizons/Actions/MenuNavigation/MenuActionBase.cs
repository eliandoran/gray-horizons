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