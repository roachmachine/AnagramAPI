using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using AnagramAPI.BL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace AnagramAPI.Controllers
{
    /// <summary>
    /// API Controller for handling anagram-related operations.
    /// </summary>
    /// <author>Michael</author>
    /// <datetime>5/25/2017 7:00 PM</datetime>
    /// <remarks>Provides endpoints for generating anagrams based on user input.</remarks>
    /// <seealso cref="Controller" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="AnagramController"/> class.
    /// </remarks>
    /// <param name="memoryCache">The memory cache for caching dictionary data.</param>
    /// <param name="db">The database context for accessing dictionary data.</param>
    [Route("api/[controller]")]
    public partial class AnagramController(IMemoryCache memoryCache, DictionaryDBContext db) : Controller
    {
        // Default values and constants
        private const string DefaultInput = "roachmachine";
        private const string BasicDictionaryCacheKey = "BasicEnglishDictionary";
        private const int DefaultMinWordLength = 2;
        private const int DefaultMaxNumWords = 3;
        private const int MaxInputLetters = 20;

        // Dependencies
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly DictionaryDBContext _db = db;

        /// <summary>
        /// Endpoint to generate anagrams based on the input string and constraints.
        /// </summary>
        /// <param name="input">The input string to generate anagrams for.</param>
        /// <param name="minwordlength">The minimum length of words in the anagrams. Defaults to 2.</param>
        /// <param name="maxnumwords">The maximum number of words in the anagrams. Defaults to 3.</param>
        /// <param name="psuedonymn">Reserved for future use (e.g., pseudonym search).</param>
        /// <returns>A list of anagrams or an error message if the operation fails.</returns>
        /// <exception cref="Exception">Thrown if the input exceeds the maximum allowed length.</exception>
        [HttpGet]
        public IEnumerable<string> Get([FromQuery] string input, int minwordlength = 2, int maxnumwords = 3)
        {
            // Metrics for tracking input properties and performance
            var properties = new Dictionary<string, string> { { "Word", input } };
            var measurements = new Dictionary<string, double>
            {
                { "MinWordCount", minwordlength },
                { "MaxWordLength", maxnumwords }
            };

            // Placeholder for pseudonym-related functionality
            List<string> firstnamestocheck = [];

            try
            {
                Stopwatch sw = new();
                sw.Start();

                #region Validate Input
                // Handle null or empty input by using a default value
                if (string.IsNullOrEmpty(input))
                {
                    input = DefaultInput;
                }
                else
                {
                    // Normalize input to lowercase and remove non-alphabetic characters
                    input = input.ToLower();
                    Regex rgx = MyRegex();
                    input = rgx.Replace(input, "");

                    // Ensure input length does not exceed the maximum allowed
                    if (input.Length > MaxInputLetters)
                    {
                        throw new Exception("Input greater than 15 characters");
                    }
                }

                // Validate and adjust minimum word length
                if (minwordlength <= 0)
                {
                    minwordlength = DefaultMinWordLength;
                }
                else if (minwordlength > input.Length)
                {
                    minwordlength = input.Length;
                }

                // Validate and adjust maximum number of words
                if (maxnumwords <= 0 || maxnumwords > 4)
                {
                    maxnumwords = DefaultMaxNumWords;
                }
                #endregion

                // Dictionary to store cached or fetched dictionary data
                Dictionary<string, string> dictionaryItems = [];

                // Attempt to retrieve dictionary data from cache
                if (!_memoryCache.TryGetValue(BasicDictionaryCacheKey, out dictionaryItems))
                {
                    // Fetch dictionary data from the database if not cached
                    dictionaryItems = _db.Dictionary
                        .FromSqlRaw("exec get_basic_english_dictionary")
                        .ToDictionary(kvp => kvp.Word, kvp => kvp.Word_ordered_array);

                    // Cache the fetched dictionary data
                    _memoryCache.Set(BasicDictionaryCacheKey, dictionaryItems);
                }

                // Generate anagrams using the business logic layer
                List<string> output = AnagramBL.GetAnagrams(input.Trim(), minwordlength, maxnumwords, dictionaryItems);

                // Stop the stopwatch and record elapsed time
                sw.Stop();
                output.Add($"Elapsed Time : {sw.ElapsedMilliseconds} ms");
                measurements.Add("ElapsedTime", sw.ElapsedMilliseconds);

                return output;
            }
            catch (Exception ex)
            {
                // Return an error message in case of failure
                return [$"error: {ex.Message}"];
            }
        }

        /// <summary>
        /// Compiled regular expression to remove non-alphabetic characters from input.
        /// </summary>
        /// <returns>A regex object for matching non-alphabetic characters.</returns>
        [GeneratedRegex("[^a-z]")]
        private static partial Regex MyRegex();
    }
}