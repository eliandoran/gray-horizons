namespace GrayHorizons.Actions.MenuNavigation
{
    using GrayHorizons.UI;
    using GrayHorizons.Input;
    using GrayHorizons.Logic;

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