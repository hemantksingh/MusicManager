namespace MusicManager
{
    public static class StringExtension
    {
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
    }
}
