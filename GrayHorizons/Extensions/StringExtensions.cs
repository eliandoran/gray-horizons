namespace GrayHorizons.Extensions
{
    using System;

    /// <summary>
    /// Represents a set of formatting method extensions for the <see cref="System.String"/> class, in which the instance acts as the format specifier.
    /// </summary>
    public static class StringFormattingExtensions
    {
        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of the specified object.
        /// </summary>
        /// <returns>A copy of format in which the format items have been replaced by the string representations of the corresponding arguments.</returns>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The object to format.</param>
        public static string FormatWith(this string format,
                                        object arg0)
        {
            return String.Format(format, arg0);
        }

        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of two specified objects.
        /// </summary>
        /// <returns>A copy of format in which the format items have been replaced by the string representations of the corresponding arguments.</returns>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg0">The first object to format.</param>
        /// <param name="arg1">The second object to format.</param>
        public static string FormatWith(this string format,
                                        object arg0,
                                        object arg1)
        {
            return String.Format(format, arg0, arg1);
        }

        /// <summary>
        /// Replaces one or more format items in a specified string with the string representation of three specified objects.
        /// </summary>
        /// <returns>A copy of format in which the format items have been replaced by the string representations of the corresponding arguments.</returns>
        /// <param name="format">Format.</param>
        /// <param name="arg0">A composite format string.</param>
        /// <param name="arg1">The first object to format.</param>
        /// <param name="arg2">The second object to format.</param>
        public static string FormatWith(this string format,
                                        object arg0,
                                        object arg1,
                                        object arg2)
        {
            return String.Format(format, arg0, arg1, arg2);
        }

        /// <summary>
        /// Replaces the format item in a specified string with a string representation of a corresponding object in a specified array.
        /// </summary>
        /// <returns>A copy of format in which the format items have been replaced by the string representations of the corresponding arguments.</returns>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An object array that contains zero or more objects to format.</param>
        public static string FormatWith(this string format,
                                        params object[] args)
        {
            return String.Format(format, args);
        }

        /// <summary>
        /// Replaces the format item in a specified string with a string representation of a corresponding object in a specified array.
        /// A specified parameter supplies culture-specific formatting information.
        /// </summary>
        /// <returns>A copy of format in which the format items have been replaced by the string representations of the corresponding arguments.</returns>
        /// <param name="format">A composite format string.</param>
        /// <param name="provider">Provider.</param>
        /// <param name="args">An object array that contains zero or more objects to format..</param>
        public static string FormatWith(this string format,
                                        IFormatProvider provider,
                                        object[] args)
        {
            return String.Format(provider, format, args);
        }
    }
}

