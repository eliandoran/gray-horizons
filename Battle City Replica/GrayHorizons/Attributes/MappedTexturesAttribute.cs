/*
   _____                   _    _            _                    
  / ____|                 | |  | |          (_)                   
 | |  __ _ __ __ _ _   _  | |__| | ___  _ __ _ _______  _ __  ___ 
 | | |_ | '__/ _` | | | | |  __  |/ _ \| '__| |_  / _ \| '_ \/ __|
 | |__| | | | (_| | |_| | | |  | | (_) | |  | |/ / (_) | | | \__ \
  \_____|_|  \__,_|\__, | |_|  |_|\___/|_|  |_/___\___/|_| |_|___/
                    __/ |                                         
                   |___/              © 2015 by Doran Adoris Elian
*/
using System;

namespace GrayHorizons.Attributes
{
    /// <summary>
    /// Represents a set texture names that will be loaded by the <see cref="GrayHorizons.Renderer"/> and then used to display an <see cref="GrayHorizons.Logic.ObjectBase"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class MappedTexturesAttribute: Attribute
    {
        readonly string[] textureNames;

        /// <summary>
        /// Gets the name of the textures.
        /// </summary>
        /// <value>An array of texture names that will be loaded by with a <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</value>
        public string[] TextureNames
        {
            get
            {
                return textureNames;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Attributes.MappedTexturesAttribute"/> class.
        /// </summary>
        /// <param name="textureName">The name of the texture that will be passed on to <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</param>
        public MappedTexturesAttribute(
            string textureName)
        {
            textureNames = new [] { textureName };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GrayHorizons.Attributes.MappedTexturesAttribute"/> class.
        /// </summary>
        /// <param name="textureNames">An array containing the names of the textures that will be randomly passed on to <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</param>
        public MappedTexturesAttribute(
            string[] textureNames)
        {
            this.textureNames = textureNames;
        }

        /// <summary>
        /// Gets a random texture name from the list of mapped textures.
        /// </summary>
        /// <returns>The random texture name.</returns>
        public string GetRandomTexture()
        {
            return TextureNames[new Random().Next(TextureNames.Length)];
        }
    }
}

