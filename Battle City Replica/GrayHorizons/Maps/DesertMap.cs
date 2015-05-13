using System;
using GrayHorizons.Logic;
using GrayHorizons.Attributes;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Maps
{
    [MappedTextures (@"Maps\Desert")]
    public class DesertMap: Map
    {
        public DesertMap (GameData gameData) : base (new Vector2 (4000, 4000), gameData)
        {
            Texture = gameData.MappedTextures [GetType ()];
        }
    }
}

