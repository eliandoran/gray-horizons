namespace GrayHorizons.Events
{
    using System;
    using GrayHorizons.Logic;

    public class CollideEventArgs: EventArgs
    {
        readonly ObjectBase collidedWith;

        public ObjectBase CollidedWith
        {
            get
            {
                return collidedWith;
            }
        }

        public bool PassThrough { get; set; }

        public CollideEventArgs(ObjectBase collidedWith, bool passThrough)
        {
            this.collidedWith = collidedWith;
            PassThrough = passThrough;
        }

        public CollideEventArgs(ObjectBase collidedWith)
            : this(collidedWith, false)
        {

        }
    }
}

