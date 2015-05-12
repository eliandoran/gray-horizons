using System;
using BattleCity.Sound;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Reflection;
using System.Linq;
using BattleCity.Attributes;
using System.Diagnostics;
using BattleCity.Extensions;

namespace BattleCity
{
    public class Loader
    {
        readonly ContentManager contentManager;

        public Loader (
            ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public void LoadSoundEffects ()
        {
            string[] namespaces = { "BattleCity.Sound" };

            #if DEBUG
            Debug.WriteLine ("Loading sounds...", "LOADER");
            Debug.Indent ();
            #endif

            var types = FilterTypes (namespaces, typeof(SoundAutoLoadAttribute));
            foreach (Type type in types)
            {
                #if DEBUG
                Debug.WriteLine ("Loading pack <{0}>...".FormatWith (type.Name));
                Debug.Indent ();
                #endif

                foreach (FieldInfo member in type.GetFields())
                {
                    if (member.FieldType != typeof(SoundEffect))
                        continue;

                    var attribute = member.GetCustomAttributes (typeof(MappedSoundsAttribute), true).FirstOrDefault () as MappedSoundsAttribute;
                    if (attribute == null)
                        continue;
                    
                    var soundEffect = member.GetValue (null) as SoundEffect;
                    if (soundEffect != null)
                    {
                        #if DEBUG
                        Debug.WriteLine ("{0} -> {1}".FormatWith (member.Name, attribute.SoundNames));
                        #endif

                        // TODO: Exception handling here?
                        foreach (var soundName in attribute.SoundNames)
                        {
                            soundEffect.Sounds.Add (contentManager.Load<Microsoft.Xna.Framework.Audio.SoundEffect> (soundName));
                        }                            
                    }
                }

                #if DEBUG
                Debug.Unindent ();
                #endif
            }

            #if DEBUG
            Debug.Unindent ();
            #endif
        }

        List<Type> FilterTypes (
            string[] namespaces,
            Type attributeType,
            Type classType = null)
        {
            return (from type in Assembly.GetExecutingAssembly ().GetTypes ()
                             where (
                                     (classType != null ? type == classType : type.IsClass) &&
                                     namespaces.Contains (type.Namespace) &&
                                     (type.GetCustomAttributes (attributeType, true).FirstOrDefault () != null))
                             select type).ToList ();            
        }
    }
}

