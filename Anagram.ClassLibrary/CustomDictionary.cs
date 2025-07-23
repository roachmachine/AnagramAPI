using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Anagram.ClassLibrary
{
    /// <summary>
    /// Dictionary Class
    /// </summary>
    /// <author>Michael</author>
    /// <datetime>5/25/2017 5:39 PM</datetime>
    /// <remarks>Defacto Dictionary Class to support reading and interacting with the dictionaries</remarks>
    public class CustomDictionary
    {
        /// <summary>
        /// Gets a dictionary that contains only words that exist within the input text
        /// </summary>
        /// <param name="BaseDictionaryItems">The dictionary items.</param>
        /// <param name="InputText">The input text.</param>
        /// <param name="DefaultMinWordLength">Default length of the minimum word.</param>
        /// <returns>A Dictionary that contains only words that exist within the input text</returns>
        public static Dictionary<string, List<string>> GetDictionary(Dictionary<string, string> BaseDictionaryItems, string InputText, int DefaultMinWordLength)
        {
            //holds the words associated with the word ordered array key
            List<string> wordList;

            //holds the word ordered array key and the associated words in a list
            Dictionary<string, List<string>> Dictionary = [];

            //This regex expression will eliminate any words whose distinct letters are not in the input string
            StringBuilder regexExpression = new();
            regexExpression.Append("[^").Append(InputText).Append("']");

            //Only get order word arrays that can possibly exist within the input text
            foreach (var item in BaseDictionaryItems)
            {
                //account for apostorphe in the input text (i.e cats input text will contain cat's and act's)
                int word_text_length = item.Key.Length;
                if (item.Key.Contains('\''))
                {
                    word_text_length--;
                }

                //word must be <= input text, not contain any letters not in the input text, not contain more single letters than input text, word length must be longer than the configured  minimum word length
                if (word_text_length <= InputText.Length && !Regex.IsMatch(item.Key, regexExpression.ToString(), RegexOptions.Compiled) && Helper.ValidSubstring(InputText, item.Key) && DefaultMinWordLength <= item.Key.Length)
                {
                    //save the original key (with or without apostrophe)
                    string original_key = item.Value;

                    //we'll will use ' words but we will store the key without the apostophy
                    original_key = original_key.Replace("'", "");

                    //if we don't have the key yet (ordered word array without apostophy) then add it
                    //also if new instantiate an empty word list for the value
                    if (!Dictionary.ContainsKey(original_key))
                    {
                        //create an empty word list and insert the key and the empty list
                        wordList = [];
                        Dictionary.Add(original_key.ToLower(), wordList);
                    }
                    else
                    {
                        //key already exists so grab it's word list
                        wordList = Dictionary[original_key];
                    }

                    //if the word's ordered array exists as a key, then we save the word in the list of values
                    // so the ordered word array is acr, will be stored as the key, and car and arc will be stored with that key in the word list as they have the same key.
                    if (!wordList.Contains(item.Key))
                    {
                        wordList.Add(item.Key.ToLower());
                    }
                }
            }

            //got it
            return Dictionary;
        }
    }
}