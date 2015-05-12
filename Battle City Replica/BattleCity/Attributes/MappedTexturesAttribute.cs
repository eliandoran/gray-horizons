using System;

namespace BattleCity.Attributes
{
    /// <summary>
    /// Represents a set texture names that will be loaded by the <see cref="BattleCity.Renderer"/> and then used to display an <see cref="BattleCity.Logic.ObjectBase"/>.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class MappedTexturesAttribute: Attribute
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
        /// Initializes a new instance of the <see cref="BattleCity.Attributes.MappedTexturesAttribute"/> class.
        /// </summary>
        /// <param name="textureName">The name of the texture that will be passed on to <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</param>
        public MappedTexturesAttribute (
            string textureName)
        {
            textureNames = new [] { textureName };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Attributes.MappedTexturesAttribute"/> class.
        /// </summary>
        /// <param name="textureNames">An array containing the names of the textures that will be randomly passed on to <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</param>
        public MappedTexturesAttribute (
            string[] textureNames)
        {
            this.textureNames = textureNames;
        }

        /// <summary>
        /// Gets a random texture name from the list of mapped textures.
        /// </summary>
        /// <returns>The random texture name.</returns>
        public string GetRandomTexture ()
        {
            return TextureNames [new Random ().Next (TextureNames.Length)];
        }
    }
}

