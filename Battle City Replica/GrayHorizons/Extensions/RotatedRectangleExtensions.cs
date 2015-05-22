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
using GrayHorizons.ThirdParty;
using Microsoft.Xna.Framework;

namespace GrayHorizons.Extensions
{
    public static class RotatedRectangleExtensions
    {
        public static RotatedRectangle Offset(
            this RotatedRectangle rect,
            Point offsetBy)
        {
            var newRect = new RotatedRectangle(rect.CollisionRectangle, rect.Rotation);
            var rads = rect.Rotation;

            var deltaX = new Point(
                             (int)(Math.Cos(rads) * offsetBy.X),
                             (int)(Math.Sin(rads) * offsetBy.X));
            var deltaY = new Point(
                             (int)(Math.Sin(rads) * -offsetBy.Y),
                             (int)(Math.Cos(rads) * offsetBy.Y)
                         );

            newRect.CollisionRectangle.Offset(deltaX);
            newRect.CollisionRectangle.Offset(deltaY);
            return newRect;
        }
    }
}

