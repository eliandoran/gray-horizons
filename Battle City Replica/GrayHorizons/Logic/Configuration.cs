﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Serialization;
using GrayHorizons.Extensions;
using GrayHorizons.Input;

namespace GrayHorizons.Logic
{
    [XmlRoot("GrayHorizons")]
    public class Configuration
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

        public Configuration()
        {
            inputBindings = new List<InputBinding>();
        }

        public void Save(InputOutputAgent agent, string filePath)
        {
            using (var stream = agent.GetStream(filePath, false))
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

                if (binding.BoundAction != null)
                {
                    binding.BoundAction.GameData = gameData;
                    binding.BoundAction.Player = gameData.ActivePlayer;
                }
            }
        }

        public static Configuration Load(InputOutputAgent agent, string filePath)
        {
            Configuration config = null;

            using (var stream = agent.GetStream(filePath, true))
            {
                var xml = new XmlSerializer(typeof(Configuration));

                try
                {
                    config = xml.Deserialize(stream) as Configuration;
                }
                catch (InvalidOperationException e)
                {
                    #if DEBUG
                    Debug.WriteLine("Error loading configuration.", "CONFIG");
                    Debug.WriteLine(e.ToString());
                    #endif
                }
            }

            #if DEBUG
            if (config != null)
            {
                Debug.WriteLine("Input Bindings ({0}):".FormatWith(config.InputBindings.Count), "CONFIG");

                foreach (InputBinding binding in config.InputBindings)
                    Debug.WriteLine(binding.ToString());
            }
            #endif

            return config;
        }
    }
}
