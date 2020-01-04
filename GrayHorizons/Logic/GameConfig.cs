namespace GrayHorizons.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Xml.Serialization;
    using GrayHorizons.Extensions;
    using GrayHorizons.Input;

    [XmlRoot("GrayHorizons")]
    public class GameConfig
    {
        readonly List<InputBinding> inputBindings;

        [XmlArray]
        [XmlArrayItem("Bind")]
        public List<InputBinding> InputBindings
        {
            get
            {
                return inputBindings;
            }
        }

        [XmlElement]
        public Size FullScreenResolution { get; set; }

        [XmlElement]
        public Size WindowedModeResolution { get; set; }

        [XmlElement]
        public bool FullScreen { get; set; }

        public GameConfig()
        {
            inputBindings = new List<InputBinding>();
        }

        public void Save(IInputOutputAgent agent, string filePath)
        {
            using (var stream = agent.GetStream(filePath, FileMode.Create))
            {
                var xml = new XmlSerializer(GetType());
                xml.Serialize(stream, this);
            }
        }

        public static void ConfigureGameDataInputBindings(GameData gameData)
        {
            foreach (InputBinding binding in gameData.Configuration.InputBindings)
            {
                binding.Player = gameData.ActivePlayer;

                if (binding.BoundAction.IsNotNull())
                {
                    binding.Game = gameData.Game;
                    binding.GameData = gameData;
                    binding.BoundAction.GameData = gameData;
                    binding.BoundAction.Player = gameData.ActivePlayer;
                }
            }
        }

        public static GameConfig Load(IInputOutputAgent agent, string filePath)
        {
            GameConfig config = null;

            using (var stream = agent.GetStream(filePath, FileMode.Open))
            {
                var xml = new XmlSerializer(typeof(GameConfig));

                try
                {
                    config = xml.Deserialize(stream) as GameConfig;
                }
                catch (InvalidOperationException e)
                {
                    Debug.WriteLine("Error loading configuration.", "CONFIG");
                    Debug.WriteLine(e.ToString());
                }
            }

            #if DEBUG
            if (config.IsNotNull())
            {
                Debug.WriteLine("Input Bindings ({0}):".FormatWith(config.InputBindings.Count), "CONFIG");
                config.InputBindings.ForEach(obj => Debug.WriteLine(obj));
            }
            #endif

            return config;
        }
    }
}
