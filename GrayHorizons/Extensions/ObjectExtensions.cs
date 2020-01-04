namespace GrayHorizons.Extensions
{
    using System;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a set of extensions attached to the <see cref="System.Object"/> class.
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Determines whether the specified object is null.
        /// </summary>
        /// <returns><c>true</c> if is the specified object is null; otherwise, <c>false</c>.</returns>
        /// <param name="obj">The object to check for null.</param>
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }

        /// <summary>
        /// Determines whether the specified object is not null.
        /// </summary>
        /// <returns><c>true</c> if is the specified object is not null; otherwise, <c>false</c>.</returns>
        /// <param name="obj">The object to check for not null.</param>
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        /// <summary>
        /// Attempts to cast this object to an arbitrary type <typeparamref name="T" /> and calls <paramref name="callBack"/> if it succeeded.
        /// </summary>
        /// <param name="obj">The object to type cast.</param>
        /// <param name="callBack">An <see cref="System.Action"/> with the resulted type cast as parameter.</param>
        /// <typeparam name="T">The type that the <paramref name="obj"/> will be casted to.</typeparam>
        public static void TryCast<T>(this object obj, Action<T> callBack) where T: class
        {            
            var cast = obj as T;
            if (cast.IsNotNull())
                callBack.Invoke(cast);
        }

        /// <summary>
        /// Serializes this object into the <paramref name="outputStream"/> <see cref="System.IO.Stream"/> with a custom root attribute.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="outputStream">The stream that the result of the serialization will be stored into.</param>
        /// <param name="root">The custom root attribute passed on to the <see cref="System.Xml.Serialization.XmlSerializer"/>.</param>
        public static void SerializeInto(this object obj, Stream outputStream, XmlRootAttribute root)
        {
            new XmlSerializer(obj.GetType(), root).Serialize(outputStream, obj);
        }

        /// <summary>
        /// Deserializes the contents of this <see cref="System.IO.Stream"/> to the specified <typeparamref name="T"> type with a custom root attribute.
        /// </summary>
        /// <param name="inputStream">The stream that contains the serialized object.</param>
        /// <param name="root">The custom root attribute passed on to the <see cref="System.Xml.Serialization.XmlSerializer"/>.</param>
        /// <typeparam name="T">The type of the serialized object.</typeparam>
        public static T DeserializeTo<T>(this Stream inputStream, XmlRootAttribute root)
        {
            return (T)(new XmlSerializer(typeof(T), root).Deserialize(inputStream));
        }
    }
}

