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
        public static void LoadMappedTextures(
            GameData gameData,
            bool fullReload = false)
        {
            Debug.WriteLine("MAPPED TEXTURES:");

            (from type in Assembly.GetExecutingAssembly().GetTypes()
                      where (type.IsClass && type.Namespace.StartsWith("GrayHorizons.") && (type.GetCustomAttributes(
                              typeof(MappedTexturesAttribute),
                              true).FirstOrDefault().IsNotNull()))
                      select type).ToList().ForEach(type =>
                {
                    if (!fullReload && gameData.MappedTextures.ContainsKey(type))
                        return;

                    var attr = (MappedTexturesAttribute)type.GetCustomAttributes(
                                   typeof(MappedTexturesAttribute),
                                   true).First();
                    var textureName = attr.GetRandomTexture();

                    try
                    {
                        var texture = gameData.ContentManager.Load<Texture2D>(textureName);
                        gameData.MappedTextures.Add(type, texture);
                    }
                    catch (ContentLoadException e)
                    {
                        Debug.WriteLine("Unable to map type <{0}> with \"{1}\". Texture load error:\n{2}".FormatWith(
                                type.Name,
                                textureName,
                                e.Message));
                        return;
                    }

                    Debug.WriteLine("Mapped the type of <{0}> with \"{1}\".".FormatWith(type.Name,
                            textureName));
                });
        }

        public static void LoadSoundEffects(GameData gameData)
        {
            string[] namespaces = { "GrayHorizons.Sound" };

            Debug.WriteLine("Loading sounds...", "LOADER");

            FilterTypes(namespaces, typeof(SoundAutoLoadAttribute)).ForEach(type =>
                {
                    Debug.WriteLine("Loading pack <{0}>...".FormatWith(type.Name));

                    type.GetFields().ToList().ForEach(member =>
                        {
                            if (member.FieldType != typeof(SoundEffect))
                                return;

                            var attribute = member.GetCustomAttributes(typeof(MappedSoundsAttribute), true).FirstOrDefault() as MappedSoundsAttribute;
                            if (attribute.IsNull())
                                return;

                            var soundEffect = member.GetValue(null) as SoundEffect;
                            if (soundEffect.IsNotNull())
                            {
                                Debug.WriteLine("{0} -> {1}".FormatWith(member.Name, attribute.SoundNames));

                                // TODO: Exception handling here?
                                attribute.SoundNames.ForEach(soundName =>
                                    soundEffect.Sounds.Add(gameData.Game.Content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>(soundName)));
                            }
                        });
                });
        }

        static List<Type> FilterTypes(
            IEnumerable<string> namespaces,
            Type attributeType,
            Type classType = null)
        {
            return (from type in Assembly.GetExecutingAssembly().GetTypes()
                             where (
                                     (classType.IsNotNull() ? type == classType : type.IsClass) &&
                                     namespaces.Contains(type.Namespace) &&
                                     (type.GetCustomAttributes(attributeType, true).FirstOrDefault().IsNotNull()))
                             select type).ToList();            
        }
    }
}

