using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExtensionMethods
{
    /// <summary>
    /// Some useful string extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Removes quotes for a given string.
        /// </summary>
        /// <param name="value">The needle string.</param>
        /// <returns>Returns a string without quotes.</returns>
        public static string RemoveQuotes(this string value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            var s = value.Replace("\"", "");
            return value.Replace("\"", "");
        }

        /// <summary>
        /// Removes quotes for a StringValues object.
        /// </summary>
        /// <param name="value">The given StringValues object.</param>
        /// <returns>A string without the quotes.</returns>
        public static string RemoveQuotes(this StringValues value)
        {
            if (string.IsNullOrEmpty(value)) return "";
            var s = value.ToString().Replace("\"", "");
            return value.ToString().Replace("\"", "");
        }
    }
}
