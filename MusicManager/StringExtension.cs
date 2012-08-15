using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MusicManager
{
    public static class StringExtension
    {
        /// <summary>
        /// Strips out non alphanumeric characters except non trailing whitespaces, @, - (dash) and . (a period)
        /// from a string.
        /// </summary>
        /// <param name="inputString">The string to clean.</param>
        /// <param name="stripDash">Indicates whether to strip a dash (-).</param>
        /// <param name="shouldTrim">Indicates whether to trim the string.</param>
        /// <returns>String stripped out of non alphanumeric characters.</returns>
        //public static string Clean (this string inputString, bool stripDash = false, bool shouldTrim = true)
        //{
        //    // Pattern that ignores alphanumeric characters, whitespaces, . (a period) and @. 
        //    string pattern = stripDash ? @"[^\w\s\.@]" : @"[^\w\s\.@-]";

        //    return shouldTrim ? Regex.Replace(inputString, pattern, string.Empty).Trim() :
        //        Regex.Replace(inputString, pattern, string.Empty);
        //}

        /// <summary>
        /// Removes the specified characters from the string.
        /// </summary>
        /// <param name="inputString">The string to clean.</param>
        /// <param name="charsToRemove">The characters to remove.</param>
        /// <param name="shouldTrim">Indicates whether to trim the string</param>
        /// <returns>String </returns>
        public static string Remove(this string inputString, char[] charsToRemove, bool shouldTrim = true)
        {
            string temp = string.Empty;

            foreach (char c in charsToRemove)
            {
                temp = inputString = inputString.Replace(c.ToString(), string.Empty);
            }

            return shouldTrim ? temp.Trim() : temp;
        }

        //public static string Clean(this string inputString, char charToRemove, int occurence)
        //{
        //    inputString.Remove(
        //}
    }
}
