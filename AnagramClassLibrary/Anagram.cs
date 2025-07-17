using System;
using System.Collections.Generic;
using System.Linq;

namespace AnagramClassLibrary
{
    /// <summary>
    /// Represents a class for generating and managing anagrams based on input text and dictionary constraints.
    /// </summary>
    public class Anagram
    {
        /// <summary>
        /// Gets or sets the minimum length of a word in the anagram.
        /// </summary>
        public int MinimumLengthOfWord { get; set; }

        /// <summary>
        /// Gets or sets the input text for generating anagrams.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of words allowed in an anagram.
        /// </summary>
        public int MaximumNumberOfWords { get; set; }

        /// <summary>
        /// Gets or sets the type of dictionary used for generating anagrams.
        /// </summary>
        public int DictionaryType { get; set; }

        /// <summary>
        /// List of keys derived from the dictionary for anagram matching.
        /// </summary>
        private readonly List<string> KeyList = [];

        /// <summary>
        /// Stores the running list of matched anagrams during processing.
        /// </summary>
        private readonly List<List<string>> runningListOfMatchedAnagrams = [];

        /// <summary>
        /// Dictionary containing words and their possible anagrams.
        /// </summary>
        private readonly Dictionary<string, List<string>> Dictionary = [];

        /// <summary>
        /// Initializes a new instance of the <see cref="Anagram"/> class with specified parameters.
        /// </summary>
        /// <param name="InputText">The input text for generating anagrams.</param>
        /// <param name="MinWordLength">The minimum length of a word in the anagram.</param>
        /// <param name="MaxNumWords">The maximum number of words allowed in an anagram.</param>
        /// <param name="AnagramDictionary">The dictionary used for anagram generation.</param>
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
        /// Generates all possible anagrams based on the input text and dictionary constraints.
        /// </summary>
        /// <returns>A list of all valid anagrams.</returns>
        public List<string> GetAllAnagrams()
        {
            string SortedInput = new([.. Input.OrderBy(c => c)]);

            // Iterate through the key list to find matching anagrams.
            for (int index = 0; index < KeyList.Count; index++)
            {
                FindAnagrams(index, SortedInput, []);
            }

            // Generate the final output list of anagrams.
            List<string> FinalOutput = Output();
            return FinalOutput;
        }

        /// <summary>
        /// Recursively finds anagrams by matching keys with the input text.
        /// </summary>
        /// <param name="KeyListIndex">The current index in the key list.</param>
        /// <param name="Input">The remaining input text to match.</param>
        /// <param name="AnagramSubList">The current list of matched words forming an anagram.</param>
        private void FindAnagrams(int KeyListIndex, string Input, List<string> AnagramSubList)
        {
            string searchWord = KeyList[KeyListIndex];

            // Check for an exact match with the input text.
            if (Input.Equals(searchWord))
            {
                AnagramSubList.Add(searchWord);

                // Add the completed anagram to the running list.
                List<string> FinalList = [];
                foreach (string s in AnagramSubList)
                {
                    FinalList.Add(s);
                }

                runningListOfMatchedAnagrams.Add(FinalList);
                return;
            }

            // Check if the search word can be formed from the input text.
            if (IsContained(ref Input, searchWord))
            {
                // Recursively find anagrams with the remaining input text.
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
        }

        /// <summary>
        /// Determines whether the specified input contains all characters of the given word.
        /// </summary>
        /// <param name="Input">The input text to check.</param>
        /// <param name="RemoveList">The word to check against the input text.</param>
        /// <returns><c>true</c> if the input contains all characters of the word; otherwise, <c>false</c>.</returns>
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
                    // Remove the matched character from the input text.
                    Input = Input.Remove(Input.IndexOf(Letter), 1);
                }
            }

            return true;
        }

        /// <summary>
        /// Generates the final output list of anagrams after processing.
        /// </summary>
        /// <returns>A list of formatted anagrams.</returns>
        private List<string> Output()
        {
            List<string> OutputList = [];
            List<FinalAnagram> FinalAnagrams = [];

            // Helper function to generate combinations of words.
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

            // Helper function to format combinations into strings.
            static IEnumerable<string> f(IEnumerable<IEnumerable<string>> xss)
            {
                return f0(xss, xss).Select(xs => string.Join(" ", xs));
            }

            List<string[][]> ListOfStringArrays = [];

            // Process each set of matched anagrams.
            foreach (List<string> AnagramSet in runningListOfMatchedAnagrams)
            {
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

            // Generate and filter the final list of anagrams.
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

            // Sort and format the final list of anagrams.
            FinalAnagrams.Sort();
            foreach (FinalAnagram fa in FinalAnagrams)
            {
                OutputList.Add(fa.Anagram);
            }

            return OutputList;
        }

        /// <summary>
        /// Represents a final anagram with its word count for sorting and output purposes.
        /// </summary>
        private class FinalAnagram(string StringAnagram, int NumberOfWords) : IComparable
        {
            /// <summary>
            /// Gets or sets the word count of the anagram.
            /// </summary>
            public int WordCount { get; set; } = NumberOfWords;

            /// <summary>
            /// Gets or sets the formatted anagram string.
            /// </summary>
            public string Anagram { get; set; } = StringAnagram;

            /// <summary>
            /// Compares the current instance with another object of the same type for sorting.
            /// </summary>
            /// <param name="obj">The object to compare with this instance.</param>
            /// <returns>
            /// A value indicating the relative order of the objects being compared.
            /// </returns>
            /// <exception cref="System.ArgumentException">Thrown when the parameter is not a FinalAnagram object.</exception>
            public int CompareTo(object obj)
            {
                if (obj is FinalAnagram temp)
                {
                    // Sort by word count in descending order.
                    if (this.WordCount > temp.WordCount)
                    {
                        return 1;
                    }
                    else if (this.WordCount < temp.WordCount)
                    {
                        return -1;
                    }

                    // Sort alphabetically if word counts are equal.
                    int TempStringCompare = string.Compare(this.Anagram, temp.Anagram);
                    if (TempStringCompare == 1)
                    {
                        return 1;
                    }
                    else if (TempStringCompare == -1)
                    {
                        return -1;
                    }

                    // Return 0 if tied.
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