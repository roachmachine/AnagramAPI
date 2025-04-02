using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnagramClassLibrary;

namespace AnagramAPI.BL
{
    /// <summary>
    /// BL for the dictionary
    /// </summary>
    public class DictionaryBL
    {
        internal static Dictionary<string, List<string>> GetDictionary(Dictionary<string, string> DictionaryItems, string InputTest, int DefaultMinWordLength)
        {
            return CustomDictionary.GetDictionary(DictionaryItems, InputTest, DefaultMinWordLength);
        }
    }
}
