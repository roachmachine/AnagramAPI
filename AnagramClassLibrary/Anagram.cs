using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramClassLibrary
{

    /// <summary>
    /// Anagram Class
    /// </summary>
    public class Anagram
    {
        /// <summary>
        /// Gets or sets the minimum length of word.
        /// </summary>
        /// <value>
        /// The minimum length of word.
        /// </value>
        public int MinimumLengthOfWord { get; set; }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        /// <value>
        /// The input.
        /// </value>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of words.
        /// </summary>
        /// <value>
        /// The maximum number of words.
        /// </value>
        public int MaximumNumberOfWords { get; set; }

        /// <summary>
        /// Gets or sets the type of the dictionary.
        /// </summary>
        /// <value>
        /// The type of the dictionary.
        /// </value>
        public int DictionaryType { get; set; }

        /// <summary>
        /// The key list
        /// </summary>
        readonly List<string> KeyList = [];

        /// <summary>
        /// The running list of matched anagrams
        /// </summary>
        readonly List<List<string>> runningListOfMatchedAnagrams = [];

        /// <summary>
        /// The dictionary
        /// </summary>
        readonly Dictionary<string, List<string>> Dictionary = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="Anagram"/> class.
        /// </summary>
        /// <param name="InputText">The input text.</param>
        /// <param name="MinWordLength">Minimum length of the word.</param>
        /// <param name="MaxNumWords">The maximum number words.</param>
        /// <param name="AnagramDictionary">The anagram dictionary.</param>
        public Anagram(string InputText, int MinWordLength, int MaxNumWords, Dictionary<string, List<string>> AnagramDictionary)
        {
            Input = InputText;
            MinimumLengthOfWord = MinWordLength;
            MaximumNumberOfWords = MaxNumWords;
            Dictionary = AnagramDictionary;
            KeyList = [.. Dictionary.Keys];
            KeyList.Sort();
        }

        /// <summary>
        /// Gets all anagrams.
        /// </summary>
        /// <returns>List of all anagrams</returns>
        public List<string> GetAllAnagrams()
        {
            string SortedInput = new([.. Input.OrderBy(c => c)]);

            // check for all the words in key list for anagrams
            for (int index = 0; index < KeyList.Count; index++)
            {
                FindAnagrams(index, SortedInput, []);
            }

            List<string> FinaOutput = Output();
            return FinaOutput;
        }

        /// <summary>
        /// Finds the anagrams.
        /// </summary>
        /// <param name="KeyListIndex">Index of the key list.</param>
        /// <param name="Input">The input.</param>
        /// <param name="AnagramSubList">The anagram sub list.</param>
        private void FindAnagrams(int KeyListIndex, string Input, List<string> AnagramSubList)
        {
            string searchWord = KeyList[KeyListIndex];

            //we have a winner and an exact match
            if (Input.Equals(searchWord))
            {
                //ok man we won as we are perfectly out of letters
                AnagramSubList.Add(searchWord);

                List<string> FinalList = [];
                foreach (string s in AnagramSubList)
                {
                    FinalList.Add(s);
                }

                runningListOfMatchedAnagrams.Add(FinalList);

                return;
            }

            if (IsContained(ref Input, searchWord))
            {
                //TempListOfAnagrams.Add(searchWord);
                //get the remaining string and send through the engine again
                for (int index = KeyListIndex + 1; index < KeyList.Count; index++)
                {
                    if (Input.Length >= MinimumLengthOfWord && AnagramSubList.Count <= MaximumNumberOfWords)
                    {
                        List<string> ClonedAnagramSubList = [.. AnagramSubList.Select(w => w)];
                        ClonedAnagramSubList.Add(searchWord);
                        FindAnagrams(index, Input, ClonedAnagramSubList);
                    }
                    else
                    {
                        return;
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Determines whether the specified input is contained.
        /// </summary>
        /// <param name="Input">The input.</param>
        /// <param name="RemoveList">The remove list.</param>
        /// <returns>
        ///   <c>true</c> if the specified input is contained; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsContained(ref string Input, string RemoveList)
        {
            string OriginalInput = Input;

            foreach (char Letter in RemoveList.ToCharArray())
            {
                if (!Input.Contains<char>(Letter))
                {
                    Input = OriginalInput;
                    return false;
                }
                else
                {
                    //improve if possible
                    Input = Input.Remove(Input.IndexOf(Letter), 1);
                }
            }

            return true;
        }

        /// <summary>
        /// Outputs this instance.
        /// </summary>
        /// <returns></returns>
        private List<string> Output()
        {
            List<string> OutputList = [];
            List<FinalAnagram> FinalAnagrams = [];

            static IEnumerable<IEnumerable<string>> f0(IEnumerable<IEnumerable<string>> xss, IEnumerable<IEnumerable<string>> xss2)
            {
                if (!xss.Any())
                {
                    return [[]];
                }
                else
                {
                    var query =
                        from x in xss.First()
                        from y in f0(xss.Skip(1), xss)
                        select new[] { x }.Concat(y);
                    return query;
                }
            }

            static IEnumerable<string> f(IEnumerable<IEnumerable<string>> xss)
            {
                return f0(xss, xss).Select(xs => string.Join(" ", xs));
            }

            List<string[][]> ListOfStringArrays = [];

            foreach (List<string> AnagramSet in runningListOfMatchedAnagrams)
            {
                //each line contains a row of index words
                int counter = 0;
                string[][] Outer = new string[AnagramSet.Count][];
                foreach (string s in AnagramSet)
                {
                    string[] inner = [.. Dictionary[s]];
                    Outer[counter] = inner;
                    counter++;
                }

                ListOfStringArrays.Add(Outer);
            }

            foreach (var v in ListOfStringArrays)
            {
                var results = f(v);
                foreach (var anagram in results)
                {
                    string[] SortedAnagram = anagram.Split(' ');
                    if (SortedAnagram.Length <= MaximumNumberOfWords)
                    {
                        bool minWordsPassed = true;
                        foreach (string s in SortedAnagram)
                        {
                            //Minimumn word length reequirement
                            if (s.Length < MinimumLengthOfWord)
                            {
                                minWordsPassed = false;
                                break;
                            }
                        }
                        if (minWordsPassed)
                        {
                            Array.Sort(SortedAnagram);
                            FinalAnagrams.Add(new FinalAnagram(String.Join(" ", SortedAnagram), SortedAnagram.Length));
                        }
                    }
                }
            }

            //sort me
            FinalAnagrams.Sort();
            foreach (FinalAnagram fa in FinalAnagrams)
            {
                OutputList.Add(fa.Anagram);
            }

            return OutputList;
        }

        /// <summary>
        /// FinalAnagram
        /// </summary>
        /// <seealso cref="System.IComparable" />
        /// <remarks>
        /// Initializes a new instance of the <see cref="FinalAnagram"/> class.
        /// </remarks>
        /// <param name="StringAnagram">The string anagram.</param>
        /// <param name="NumberOfWords">The number of words.</param>
        private class FinalAnagram(string StringAnagram, int NumberOfWords) : IComparable
        {
            /// <summary>
            /// Gets or sets the word count.
            /// </summary>
            /// <value>
            /// The word count.
            /// </value>
            public int WordCount { get; set; } = NumberOfWords;

            /// <summary>
            /// Gets or sets the anagram.
            /// </summary>
            /// <value>
            /// The anagram.
            /// </value>
            public string Anagram { get; set; } = StringAnagram;

            /// <summary>
            /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
            /// </summary>
            /// <param name="obj">An object to compare with this instance.</param>
            /// <returns>
            /// A value that indicates the relative order of the objects being compared. The return value has these meanings:
            /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="obj" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="obj" /> in the sort order.</description></item></list>
            /// </returns>
            /// <exception cref="System.ArgumentException">Parameter is not an Anagram object</exception>
            public int CompareTo(object obj)
            {
                //use pattern matching here
                if (obj is FinalAnagram)
                {
                    var temp = obj as FinalAnagram;

                    //first sort by word count desc
                    if (this.WordCount > temp.WordCount)
                    {
                        return 1;
                    }
                    else if ((this.WordCount < temp.WordCount))
                    {
                        return -1;
                    }

                    //then sort by alphabet
                    int TempStringCompare = string.Compare(this.Anagram, temp.Anagram);
                    if (TempStringCompare == 1)
                    {
                        return 1;
                    }
                    else if (TempStringCompare == -1)
                    {
                        return -1;
                    }

                    //tied
                    return 0;
                }
                else
                {
                    throw new ArgumentException("Parameter is not an Anagram object");
                }
            }
        }
    }
}