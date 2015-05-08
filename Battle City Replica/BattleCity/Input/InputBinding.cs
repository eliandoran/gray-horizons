using System;
using BattleCity.Logic;
using System.Xml.Serialization;
using System.Reflection;
using System.Diagnostics;
using BattleCity.Extensions;
using BattleCity.Attributes;

namespace BattleCity.Input
{
    [XmlInclude (typeof(KeyBinding))]
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
                    AllowContinousPress = (boundAction.GetType ().GetCustomAttributes (typeof(AllowContinousPress),
                                                                                       true).Length > 0);
                    Debug.WriteLine (ToString () + " " + AllowContinousPress);
                }
            }
        }

        [XmlAttribute ("with")]
        public string BoundActionType
        {
            get
            {
                return boundAction.GetType ().Name;
            }
            set
            {
                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    if (type.Name == value)
                    {
                        #if DEBUG
                        Debug.WriteLine ("Found the bound action for <{1}>.".FormatWith (ToString (), value),
                                         "ACTIONS");
                        #endif

                        BoundAction = type.GetConstructor (new Type[] { }).Invoke (new object[] { }) as GameAction;
                    }
                }
            }
        }

        [XmlIgnore ()]
        public Player Player { get; set; }

        [XmlIgnore ()]
        public bool AllowContinousPress { get; set; }

        GameData GameData
        {
            get
            {
                return gameData;
            }
        }

        internal InputBinding (GameData gameData,
                               GameAction boundAction,
                               Player player,
                               bool allowContinousPress)
        {
            this.gameData = gameData;
            AllowContinousPress = allowContinousPress;
            BoundAction = boundAction;
            Player = player;
        }

        internal InputBinding (GameData gameData,
                               GameAction boundAction,
                               bool allowContinousPress) : this (gameData,
                                                                 boundAction,
                                                                 null,
                                                                 allowContinousPress)
        {
            
        }

        internal InputBinding (GameData gameData) : this (gameData,
                                                          null,
                                                          false)
        {

        }

        public abstract void UpdateState();

        public abstract bool IsActive();

        public void RunIfActive()
        {
            if (IsActive ())
                BoundAction.Execute ();
        }
    }
}

