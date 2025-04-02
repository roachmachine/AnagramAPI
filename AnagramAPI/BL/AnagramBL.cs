using System.Collections.Generic;
using System.Linq;
using AnagramClassLibrary;

namespace AnagramAPI.BL
{
    /// <summary>
    /// Class to support the Business logic of the Anangram Functionality
    /// </summary>
    /// <author>Michael</author>
    /// <datetime>5/25/2017 7:02 PM</datetime>
    /// <remarks>Class to support the Business logic of the Anangram Functionality</remarks>
    internal class AnagramBL
    {
        /// <summary>
        /// Gets the anagrams.
        /// </summary>
        /// <param name="InputText">The input text.</param>
        /// <param name="MinimumWordSize">Minimum size of the word.</param>
        /// <param name="MaxNumWords">The maximum number words.</param>
        /// <param name="dictionaryItems">The dictionary items.</param>
        /// <returns></returns>
        internal static List<string> GetAnagrams(string InputText, int MinimumWordSize, int MaxNumWords, Dictionary<string, string> dictionaryItems)
        {
            //Instantiate our anagram class
            Anagram anagramHelper = new(InputText, MinimumWordSize, MaxNumWords, CustomDictionary.GetDictionary(dictionaryItems, InputText, MinimumWordSize));

            //Get all anagrams from the input
            List<string> AnagramOutput = anagramHelper.GetAllAnagrams();

            return AnagramOutput;
        }
    }
}
