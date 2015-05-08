using System;

namespace BattleCity.Attributes
{
    /// <summary>
    /// Represents the texture that will be used by the <see cref="BattleCity.Renderer"/> to display an <see cref="BattleCity.Logic.ObjectBase"/>.
    /// </summary>
    [AttributeUsage (AttributeTargets.Class)]
    public class MappedTextures: Attribute
    {
        readonly string[] textureNames;

        /// <summary>
        /// Gets the name of the texture.
        /// </summary>
        /// <value>The name of the texture that will be passed on to <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</value>
        public string[] TextureNames
        {
            get
            {
                return textureNames;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.MappedTextures"/> class.
        /// </summary>
        /// <param name="textureName">The name of the texture that will be passed on to <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</param>
        public MappedTextures (string textureName)
        {
            textureNames = new [] { textureName };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleCity.Logic.MappedTextures"/> class.
        /// </summary>
        /// <param name="textureNames">The name of the textures that will be randomly passed on to <see cref="Microsoft.Xna.Framework.Content.ContentManager"/>.</param>
        public MappedTextures (string[] textureNames)
        {
            this.textureNames = textureNames;
        }

        /// <summary>
        /// Gets a random texture from the list of mapped textures.
        /// </summary>
        /// <returns>The random texture.</returns>
        public string GetRandomTexture()
        {
            var random = new Random ();
            return TextureNames [random.Next (TextureNames.Length)];
        }
    }
}

