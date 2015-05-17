using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.Linq;
using System.Diagnostics;
using GrayHorizons.Logic;
using GrayHorizons.Attributes;
using GrayHorizons.Extensions;
using GrayHorizons.Sound;

namespace GrayHorizons
{
    public static class Loader
    {
        /// <summary>
        /// Loads the mapped textures.
        /// </summary>
        public static void LoadMappedTextures (
            GameData gameData,
            bool fullReload = false)
        {
            var namespaces = new List<string> () { "GrayHorizons.Entities", "GrayHorizons.StaticObjects" };

            var query = from type in Assembly.GetExecutingAssembly ().GetTypes ()
                                 where (type.IsClass && type.Namespace.StartsWith ("GrayHorizons.") && (type.GetCustomAttributes (
                                         typeof(MappedTexturesAttribute),
                                         true).FirstOrDefault () != null))
                                 select type;

            #if DEBUG
            Debug.WriteLine ("MAPPED TEXTURES:");
            ;
            #endif

            foreach (var type in query.ToList ())
            {
                if (!fullReload && gameData.MappedTextures.ContainsKey (type))
                    continue;

                var attr = (MappedTexturesAttribute)type.GetCustomAttributes (
                               typeof(MappedTexturesAttribute),
                               true).First ();
                var textureName = attr.GetRandomTexture ();

                try
                {
                    var texture = gameData.ContentManager.Load<Texture2D> (textureName);
                    gameData.MappedTextures.Add (type, texture);
                }
                catch (ContentLoadException e)
                {
                    #if DEBUG
                    Debug.WriteLine ("Unable to map type <{0}> with \"{1}\". Texture load error:\n{2}".FormatWith (
                            type.Name,
                            textureName,
                            e.Message));                    
                    #endif

                    throw;
                }

                #if DEBUG
                Debug.WriteLine ("Mapped the type of <{0}> with \"{1}\".".FormatWith (type.Name,
                                                                                      textureName));
                #endif
            }
        }

        public static void LoadSoundEffects (GameData gameData)
        {
            string[] namespaces = { "GrayHorizons.Sound" };

            #if DEBUG
            Debug.WriteLine ("Loading sounds...", "LOADER");
            #endif

            var types = FilterTypes (namespaces, typeof(SoundAutoLoadAttribute));
            foreach (Type type in types)
            {
                #if DEBUG
                Debug.WriteLine ("Loading pack <{0}>...".FormatWith (type.Name));
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
                            soundEffect.Sounds.Add (gameData.Game.Content.Load<Microsoft.Xna.Framework.Audio.SoundEffect> (soundName));
                        }                            
                    }
                }
            }
        }

        static List<Type> FilterTypes (
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

