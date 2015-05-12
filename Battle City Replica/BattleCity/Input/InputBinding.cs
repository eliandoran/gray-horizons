using System;
using BattleCity.Logic;
using System.Xml.Serialization;
using System.Reflection;
using System.Diagnostics;
using BattleCity.Extensions;
using BattleCity.Attributes;
using System.ComponentModel;

namespace BattleCity.Input
{
    [XmlInclude (typeof(KeyBinding))]
    [XmlInclude (typeof(AxisBinding))]
    public abstract class InputBinding
    {
        readonly GameData gameData;
        GameAction boundAction;

        [XmlIgnore ()]
        public GameAction BoundAction
        {
            get
            {
                return boundAction;
            }
            set
            {
                boundAction = value;

                if (boundAction != null)
                {
                    boundAction.ParentInputBinding = this;
                    var customAttrs = boundAction.GetType ().GetCustomAttributes (typeof(AllowContinousPressAttribute),
                                          true);

                    if (customAttrs != null)
                    {
                        AllowContinousPress = (customAttrs.Length > 0);
                    }
                }
            }
        }

        [XmlAttribute ("with")]
        public string BoundActionType
        {
            get
            {
                if (boundAction != null)
                    return boundAction.GetType ().Name;
                else
                    return String.Empty;
            }
            set
            {
                if (value == String.Empty)
                    boundAction = null;

                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (type.Name == value)
                    {
                        #if DEBUG
                        Debug.WriteLine ("Found the bound action for <{1}>.".FormatWith (ToString (), value),
                            "ACTIONS");
                        #endif

                        try
                        {
                            var constructor = type.GetConstructor (new Type[] { });
                            BoundAction = constructor.Invoke (new object[] { }) as GameAction;
                        }
                        catch (TargetParameterCountException)
                        {
                            #if DEBUG
                            Debug.WriteLine ("No suitable constructor for the bound action <{0}> has been found.".FormatWith (value));
                            #endif
                        }
                    }
                }
            }
        }

        [XmlIgnore]
        public Player Player { get; set; }

        [XmlAttribute ("player")]
        [DefaultValue (-1)]
        public int PlayerID
        {
            get
            {
                return (Player == null ? -1 : 1);
            }
            set
            {
                if (GameData != null && GameData.ActivePlayer != null)
                    Player = GameData.ActivePlayer;
            }
        }

        [XmlIgnore ()]
        public bool AllowContinousPress { get; set; }

        GameData GameData
        {
            get
            {
                return gameData;
            }
        }

        internal InputBinding (
            GameData gameData,
            GameAction boundAction,
            Player player,
            bool allowContinousPress)
        {
            this.gameData = gameData;
            AllowContinousPress = allowContinousPress;
            BoundAction = boundAction;
            Player = player;
        }

        internal InputBinding (
            GameData gameData,
            GameAction boundAction,
            bool allowContinousPress) : this (
                gameData,
                boundAction,
                null,
                allowContinousPress)
        {
            
        }

        internal InputBinding (
            GameData gameData) : this (
                gameData,
                null,
                false)
        {

        }

        public abstract void UpdateState ();

        public abstract bool IsActive ();

        public void RunIfActive ()
        {
            if (IsActive ())
                BoundAction.Execute ();
        }
    }
}

