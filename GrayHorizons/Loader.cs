using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using GrayHorizons.Attributes;
using GrayHorizons.Extensions;
using GrayHorizons.Logic;
using GrayHorizons.Sound;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

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

            Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(
                type => (type.IsClass && type.Namespace.StartsWith("GrayHorizons.", StringComparison.Ordinal) &&
                (type.GetCustomAttributes(typeof(MappedTexturesAttribute), true).FirstOrDefault().IsNotNull())))
                .ToList()
                .ForEach(type =>
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
            Debug.Indent();

            FilterTypes(namespaces, typeof(SoundAutoLoadAttribute)).ForEach(type =>
                {
                    Debug.WriteLine("Loading pack <{0}>...".FormatWith(type.Name));
                    Debug.Indent();

                    type.GetProperties().ToList().ForEach(member =>
                        member.GetCustomAttributes(typeof(MappedSoundsAttribute), true).FirstOrDefault().TryCast<MappedSoundsAttribute>(attribute =>
                            {
                                Debug.Write("{0} -> {1} ".FormatWith(member.Name, attribute));
                                var soundEffect = new SoundEffect();
                                member.SetValue(null, soundEffect, null);
                                // TODO: Exception handling here?
                                attribute.SoundNames.ForEach(soundName => soundEffect.Sounds.Add(gameData.Game.Content.Load<Microsoft.Xna.Framework.Audio.SoundEffect>(soundName)));
                                Debug.WriteLine("[Done]");
                            })
                    );

                    Debug.Unindent();
                });

            Debug.Unindent();
        }

        static List<Type> FilterTypes(
            IEnumerable<string> namespaces,
            Type attributeType,
            Type classType = null)
        {
            return (Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(type =>
                ((classType.IsNotNull() ? type == classType : type.IsClass) &&
                namespaces.Contains(type.Namespace) &&
                (type.GetCustomAttributes(attributeType, true).FirstOrDefault().IsNotNull())))
            ).ToList();            
        }
    }
}

