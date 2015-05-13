using System;
using GrayHorizons.Input;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

namespace GrayHorizons.Logic
{
    public abstract class GameAction
    {
        public event EventHandler<EventArgs> ParentInputBindingChanged;
        public event EventHandler<EventArgs> ActionExecuted;

        InputBinding parentInputBinding;

        [XmlIgnore ()]
        public Player Player { get; set; }

        [XmlIgnore ()]
        public GameData GameData { get; set; }

        [XmlIgnore ()]
        public InputBinding ParentInputBinding
        {
            get
            {
                return parentInputBinding;
            }
            set
            {
                parentInputBinding = value;
                OnParentInputBindingChanged (EventArgs.Empty);
            }
        }

        internal GameAction (
            GameData gameData = null,
            Player player = null,
            InputBinding parentInputBinding = null)
        {
            GameData = gameData;


            if (player != null)
                Player = player;
            else if (GameData != null)
                Player = gameData.ActivePlayer;
            
            ParentInputBinding = parentInputBinding;
        }

        internal GameAction (
            Player player) : this (
                null,
                player,
                null)
        {
            
        }

        internal GameAction () : this (
                null)
        {
            
        }

        protected void OnParentInputBindingChanged (
            EventArgs e)
        {
            if (ParentInputBindingChanged != null)
                ParentInputBindingChanged (this, e);
        }

        protected void OnActionExecuted (
            EventArgs e)
        {
            if (ActionExecuted != null)
                ActionExecuted (this, e);
        }

        public virtual void Execute ()
        {
            OnActionExecuted (EventArgs.Empty);
        }
    }
}

