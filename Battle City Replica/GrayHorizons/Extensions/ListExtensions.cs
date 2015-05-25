namespace GrayHorizons.Extensions
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a set of extensions attached to the <see cref="System.Collections.Generic.List&lt;T&gt;"/> class
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// A random generator to be used by the extensions defined here.
        /// </summary>
        static readonly Random random = new Random();

        /// <summary>
        /// Returns a random element from the given <see cref="System.Collections.Generic.List&lt;T&gt;"/>
        /// </summary>
        /// <returns>A random element from the given <see cref="System.Collections.Generic.List&lt;T&gt;"/> .</returns>
        /// <param name="list">The <see cref="System.Collections.Generic.List&lt;T&gt;"/> containing one or more elements.</param>
        /// <typeparam name="T">The type of the elements contained in the <see cref="System.Collections.Generic.List&lt;T&gt;"/>.</typeparam>
        public static T RandomElement<T>(this List<T> list)
        {
            return list[random.Next(list.Count)];            
        }
    }
}

