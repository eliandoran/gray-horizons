namespace GrayHorizons.Extensions
{
    using System;
    using System.Collections.Generic;

    public static class ListExtensions
    {
        static readonly Random random = new Random();

        public static T RandomElement<T>(this List<T> list)
        {
            return list[random.Next(list.Count)];
        }
    }
}

