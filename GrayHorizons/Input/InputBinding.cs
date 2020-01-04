using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;
using GrayHorizons.Attributes;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Input
{
    [XmlInclude(typeof(KeyBinding))]
    [XmlInclude(typeof(AxisBinding))]
    public abstract class InputBinding
    {
        GameAction boundAction;

        [XmlIgnore]
        public Game Game { get; set; }

        [XmlIgnore]
        public GameData GameData { get; set; }

        [XmlIgnore]
        public GameAction BoundAction
        {
            get
            {
                return boundAction;
            }
            set
            {
                boundAction = value;

                if (boundAction.IsNotNull())
                {
                    boundAction.ParentInputBinding = this;
                    var customAttrs = boundAction.GetType().GetCustomAttributes(typeof(AllowContinuousPressAttribute),
                                          true);

                    if (customAttrs.IsNotNull())
                    {
                        AllowContinuousPress = (customAttrs.Length > 0);
                    }
                }
            }
        }

        [XmlAttribute("with")]
        public string BoundActionType
        {
            get
            {
                if (boundAction.IsNotNull())
                    return boundAction.GetType().Name;
                else
                    return String.Empty;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    boundAction = null;

                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (type.Name == value)
                    {
                        Debug.WriteLine("Found the bound action for <{1}>.".FormatWith(ToString(), value),
                            "ACTIONS");

                        try
                        {
                            var constructor = type.GetConstructor(new Type[] { });
                            BoundAction = constructor.Invoke(new object[] { }) as GameAction;
                        }
                        catch (TargetParameterCountException)
                        {
                            Debug.WriteLine("No suitable constructor for the bound action <{0}> has been found.".FormatWith(value));
                        }
                    }
                }
            }
        }

        [XmlIgnore]
        public Player Player { get; set; }

        [XmlAttribute("player")]
        [DefaultValue(-1)]
        public int PlayerId
        {
            get
            {
                return (Player.IsNull() ? -1 : 1);
            }
            set
            {
                if (GameData.IsNotNull() && GameData.ActivePlayer.IsNotNull())
                    Player = GameData.ActivePlayer;
            }
        }

        [XmlIgnore()]
        public bool AllowContinuousPress { get; set; }

        internal InputBinding(
            GameData gameData,
            GameAction boundAction,
            Player player,
            bool allowContinousPress)
        {
            GameData = gameData;
            AllowContinuousPress = allowContinousPress;
            BoundAction = boundAction;
            Player = player;
        }

        internal InputBinding(
            GameData gameData,
            GameAction boundAction,
            bool allowContinousPress)
            : this(
                gameData,
                boundAction,
                null,
                allowContinousPress)
        {
            
        }

        internal InputBinding(
            GameData gameData)
            : this(
                gameData,
                null,
                false)
        {

        }

        public abstract void UpdateState();

        public abstract bool IsActive();

        public void RunIfActive()
        {
            if (IsActive())
                BoundAction.Execute();
        }
    }
}

