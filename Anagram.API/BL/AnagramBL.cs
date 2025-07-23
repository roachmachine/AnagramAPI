using System.Collections.Generic;
using Anagram.ClassLibrary;

namespace Anagram.API.BL
{
    /// <summary>
    /// Class to implement anagram business logic
    /// </summary>
    internal class AnagramBL
    {
        /// <summary>
        /// Gets the anagrams.
        /// </summary>
        /// <param name="InputText">The input text.</param>
        /// <param name="MinimumWordSize">Minimum size of the word.</param>
        /// <param name="MaxNumWords">The maximum number words.</param>
        /// <param name="dictionaryItems">The dictionary items.</param>
        /// <returns>A list of anagrams generated from the input text.</returns>
        internal static List<string> GetAnagrams(string InputText, int MinimumWordSize, int MaxNumWords, Dictionary<string, string> dictionaryItems)
        {
            // Instantiate the Anagram class with the provided input and constraints
            ClassLibrary.Anagram anagramHelper = new(InputText, MinimumWordSize, MaxNumWords, CustomDictionary.GetDictionary(dictionaryItems, InputText, MinimumWordSize));

            // Generate all possible anagrams based on the input text and dictionary
            List<string> AnagramOutput = anagramHelper.GetAllAnagrams();

            // Return the list of generated anagrams
            return AnagramOutput;
        }
    }
}
