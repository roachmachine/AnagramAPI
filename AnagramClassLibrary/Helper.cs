using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnagramClassLibrary
{
    internal static class Helper
    {
        /// <summary>
        /// Ensures the count of distinct characters of the substring can fit in the input string
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="substring">The substring.</param>
        /// <returns></returns>
        internal static bool ValidSubstring(string input, string substring)
        {
            foreach (char c in substring.Distinct())
            {
                if (c != '\'')
                {
                    if ((substring.Count(x => x == c) > input.Count(x => x == c)))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
