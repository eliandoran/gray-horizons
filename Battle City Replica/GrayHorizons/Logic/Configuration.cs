using System;
using GrayHorizons.Input;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using GrayHorizons.Extensions;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Logic
{
    [XmlRoot ("GrayHorizons")]
    public class Configuration
    {
        readonly List<InputBinding> inputBindings;

        [XmlArray ()]
        [XmlArrayItem ("Bind")]
        public List<InputBinding> InputBindings
        {
            get
            {
                return inputBindings;
            }
        }

        [XmlElement ()]
        public Size FullScreenResolution { get; set; }

        public Configuration ()
        {
            inputBindings = new List<InputBinding> ();
        }

        public void Save(string filePath)
        {
            using (var fs = new FileStream (filePath, FileMode.Create))
            {
                var xml = new XmlSerializer (GetType ());
                xml.Serialize (fs, this);
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

        public static Configuration Load(string filePath)
        {
            Configuration config = null;

            using (var fs = new FileStream (filePath, FileMode.Open))
            {
                var xml = new XmlSerializer (typeof(Configuration));

                try
                {
                    config = xml.Deserialize (fs) as Configuration;
                }
                catch (InvalidOperationException e)
                {
                    #if DEBUG
                    Debug.WriteLine ("Error loading configuration.", "CONFIG");
                    Debug.Indent ();
                    Debug.WriteLine (e.ToString ());
                    Debug.Unindent ();
                    #endif
                }
            }

            #if DEBUG
            if (config != null)
            {
                Debug.WriteLine ("Input Bindings ({0}):".FormatWith (config.InputBindings.Count), "CONFIG");
                Debug.Indent ();

                foreach (InputBinding binding in config.InputBindings)
                    Debug.WriteLine (binding.ToString ());                

                Debug.Unindent ();
            }
            #endif

            return config;
        }
    }
}
