using System;
using BattleCity.Input;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using BattleCity.Extensions;

namespace BattleCity.Logic
{
    [XmlRoot ("BattleCity")]
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

        public static Configuration Load(string filePath)
        {
            Configuration config;

            using (var fs = new FileStream (filePath, FileMode.Open))
            {
                var xml = new XmlSerializer (typeof(Configuration));
                config = xml.Deserialize (fs) as Configuration;
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
