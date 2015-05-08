using System;
using BattleCity.Logic;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.Reflection;
using BattleCity.Attributes;
using System.Xml.Serialization;

namespace BattleCity.Input
{
    public delegate void KeyPressedEventHandler (object sender, EventArgs e);
    public delegate void KeyDownEventHandler (object sender, EventArgs e);
    public delegate void KeyUpEventHandler (object sender, EventArgs e);

    public class KeyBinding: InputBinding
    {
        public event KeyPressedEventHandler KeyPressed;
        public event KeyDownEventHandler KeyDown;
        public event KeyUpEventHandler KeyUp;

        [XmlAttribute ("key")]
        public Keys Key { get; set; }

        [XmlIgnore ()]
        public bool IsPressed { get; set; }

        public KeyBinding () : this (null)
        {
            
        }

        public KeyBinding (GameData gameData,
                           GameAction action = null,
                           Keys key = Keys.None,
                           bool allowContinousPress = false) : base (gameData,
                                                                     action,
                                                                     allowContinousPress)
        {
            if (key == Keys.None && action != null)
            {
                foreach (Attribute attr in action.GetType ().GetCustomAttributes (true))
                {
                    var defaultKeyAttr = attr as DefaultKey;
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

            AllowContinousPress = allowContinousPress;
        }

        public override void UpdateState()
        {
            if (AllowContinousPress)
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

        public override bool IsActive()
        {
            if (Keyboard.GetState ().IsKeyDown (Key))
            {                
                return true;
            }

            return false;
        }

        protected virtual void OnKeyPressed(EventArgs e)
        {
            if (KeyPressed != null)
                KeyPressed (this, e);
        }

        protected virtual void OnKeyDown(EventArgs e)
        {
            if (KeyDown != null)
                KeyDown (this, e);
        }

        protected virtual void OnKeyUp(EventArgs e)
        {
            if (KeyUp != null)
                KeyUp (this, e);
        }

        public override string ToString()
        {
            return string.Format ("[KeyBinding: Key={0}, Action={1}, AllowContinousPress={2}]",
                                  Key,
                                  BoundAction,
                                  AllowContinousPress);
        }
    }
}

