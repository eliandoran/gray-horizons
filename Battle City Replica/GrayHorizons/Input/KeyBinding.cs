using System;
using System.Xml.Serialization;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using GrayHorizons.Logic;

namespace GrayHorizons.Input
{
    public class KeyBinding: InputBinding
    {
        bool updateFailed;

        /// <summary>
        /// Occurs once the key is released after being pressed.
        /// </summary>
        public event EventHandler<EventArgs> KeyPressed;

        /// <summary>
        /// Occurs when key is being pressed.
        /// </summary>
        public event EventHandler<EventArgs> KeyDown;

        /// <summary>
        /// Occurs when key has been released.
        /// </summary>
        public event EventHandler<EventArgs> KeyUp;

        [XmlAttribute ("key")]
        public Keys Key { get; set; }

        [XmlIgnore ()]
        public bool IsPressed { get; set; }

        public KeyBinding () : this (
                null)
        {
            
        }

        public KeyBinding (
            GameData gameData,
            GameAction action = null,
            Keys key = Keys.None,
            bool allowContinuousPress = false) : base (
                gameData,
                action,
                allowContinuousPress)
        {
            if (key == Keys.None && action != null)
            {
                foreach (Attribute attr in action.GetType ().GetCustomAttributes (true))
                {
                    var defaultKeyAttr = attr as DefaultKeyAttribute;
                    if (defaultKeyAttr != null)
                    {
                        Key = defaultKeyAttr.Key;
                        break;
                    }
                }
            }
            else
            {
                Key = key;
            }

            AllowContinuousPress = allowContinuousPress;
        }

        public KeyBinding (
            GameAction action) : this (
                null,
                action) { }

        public override void UpdateState ()
        {
            if (BoundAction == null)
            {
                #if DEBUG
                if (!updateFailed)
                {
                    Debug.WriteLine ("KeyBinding failed to update because there is no bound action.", "KEY");
                    Debug.WriteLine (ToString ());

                    updateFailed = true;
                }
                #endif

                return;
            }

            if (AllowContinuousPress)
            {                
                if (IsPressed)
                {
                    if (IsActive ())
                    {
                        OnKeyDown (EventArgs.Empty);
                        BoundAction.Execute ();
                    }
                    else
                    {
                        OnKeyUp (EventArgs.Empty);
                    }
                }
            }
            else
            {                
                if (IsActive ())
                {
                    if (IsPressed)
                    {
                        OnKeyDown (EventArgs.Empty);
                    }
                    else
                    {
                        OnKeyUp (EventArgs.Empty);
                        BoundAction.Execute ();
                    }
                }                    
            }

            IsPressed = IsActive ();
        }

        public override bool IsActive ()
        {
            if (Keyboard.GetState ().IsKeyDown (Key))
            {                
                return true;
            }

            return false;
        }

        /// <summary>
        /// Raises the key pressed event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> containing information for this event.</param>
        protected virtual void OnKeyPressed (
            EventArgs e)
        {
            if (KeyPressed != null)
                KeyPressed (this, e);
        }

        /// <summary>
        /// Raises the key down event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> containing information for this event.</param>
        protected virtual void OnKeyDown (
            EventArgs e)
        {
            if (KeyDown != null)
                KeyDown (this, e);
        }

        /// <summary>
        /// Raises the key up event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> containing information for this event.</param>
        protected virtual void OnKeyUp (
            EventArgs e)
        {
            if (KeyUp != null)
                KeyUp (this, e);
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.KeyBinding"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="GrayHorizons.Input.KeyBinding"/>.</returns>
        public override string ToString ()
        {
            return string.Format ("[KeyBinding: Key={0}, Action={1}, AllowContinousPress={2}]",
                                  Key,
                                  BoundAction,
                                  AllowContinuousPress);
        }
    }
}

