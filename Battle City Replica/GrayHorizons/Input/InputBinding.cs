using System;
using GrayHorizons.Logic;
using System.Xml.Serialization;
using System.Reflection;
using System.Diagnostics;
using GrayHorizons.Extensions;
using GrayHorizons.Attributes;
using System.ComponentModel;

namespace GrayHorizons.Input
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
                    var customAttrs = boundAction.GetType ().GetCustomAttributes (typeof(AllowContinuousPressAttribute),
                                                                                  true);

                    if (customAttrs != null)
                    {
                        AllowContinuousPress = (customAttrs.Length > 0);
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
                if (String.IsNullOrEmpty (value))
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
        public bool AllowContinuousPress { get; set; }

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
            AllowContinuousPress = allowContinousPress;
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

