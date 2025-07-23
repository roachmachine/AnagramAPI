using System.Collections.Generic;
using Anagram.ClassLibrary;

namespace Anagram.API.BL
{
    /// <summary>
    /// BL for the dictionary
    /// </summary>
    public class DictionaryBL
    {
        /// <summary>
        /// Retrieves a custom dictionary based on the provided dictionary items, input test, and minimum word length.
        /// </summary>
        /// <param name="DictionaryItems">A dictionary where the key is a string and the value is another string.</param>
        /// <param name="InputTest">The input string to test against the dictionary.</param>
        /// <param name="DefaultMinWordLength">The default minimum word length for filtering dictionary entries.</param>
        /// <returns>A dictionary where the key is a string and the value is a list of strings.</returns>
        internal static Dictionary<string, List<string>> GetDictionary(Dictionary<string, string> DictionaryItems, string InputTest, int DefaultMinWordLength)
        {
            // Calls the CustomDictionary class to generate a filtered dictionary.
            return CustomDictionary.GetDictionary(DictionaryItems, InputTest, DefaultMinWordLength);
        }
    }
}
