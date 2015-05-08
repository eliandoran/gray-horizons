using System;
using Microsoft.Xna.Framework.Input;

namespace BattleCity.Attributes
{
    [AttributeUsage (AttributeTargets.Class)]
    public class DefaultKey: Attribute
    {
        readonly Keys key;

        public Keys Key
        {
            get
            {
                return key;
            }
        }

        public DefaultKey (Keys key)
        {
            this.key = key;
        }
    }
}
    