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
    /// Controller class
    /// </summary>
    /// <author>Michael</author>
    /// <datetime>5/25/2017 7:00 PM</datetime>
    /// <remarks>Controller class</remarks>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public partial class AnagramController : Controller
    {
        const string DefaultInput = "michaelroach";
        const string BasicDictionaryCacheKey = "BasicEnglishDictionary";
        const int DefaultMinWordLength = 2;
        const int DefaultMaxNumWords = 4;

        //TESTURLs
        //http://localhost:56738/api/anagram?input=windmill
        //http://localhost:56738/api/anagram/?input=michaelroach&minwordlength=2&maxnumwords=2

        private readonly IMemoryCache _memoryCache;
        private readonly DictionaryDBContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnagramController" /> class.
        /// </summary>
        /// <param name="memoryCache">The memory cache.</param>
        /// <param name="db">The database.</param>
        public AnagramController(IMemoryCache memoryCache, DictionaryDBContext db)
        {
            _memoryCache = memoryCache;
            _db = db;
        }

        /// <summary>
        /// Gets the specified input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="minwordlength">The minwordlength.</param>
        /// <param name="maxnumwords">The maxnumwords.</param>
        /// <param name="psuedonymn">The psuedonymn.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Input greater than 15 characters</exception>
        [HttpGet]
        public IEnumerable<string> Get([FromQuery] string input, int minwordlength = 2, int maxnumwords = 3, int psuedonymn = 0)
        {
            // Set up some properties for metics
            var properties = new Dictionary<string, string>
                {{"Word", input}};

            var measurements = new Dictionary<string, double>
            {
                { "MinWordCount", minwordlength },
                { "MaxWordLength", maxnumwords }
            };

            List<string> firstnamestocheck = new(); //will store first names is psuedonym search

            try
            {
                Stopwatch sw = new();
                sw.Start();

                #region Validate Input
                if (input == null || input == string.Empty)
                {
                    input = DefaultInput;
                }
                else
                {
                    input = input.ToLower();
                    Regex rgx = MyRegex();
                    input = rgx.Replace(input, "");

                    if (input.Length > 15)
                    {
                        throw new Exception("Input greater than 15 characters");
                    }
                }

                //validate minimum word length less than zero and greater than the length of the input
                if (minwordlength <= 0)
                {
                    minwordlength = DefaultMinWordLength;
                }
                else if (minwordlength > input.Length)
                {
                    minwordlength = input.Length;
                }

                //maximum number of words must be at least 1 and 4 or less
                if (maxnumwords <= 0 || maxnumwords > 4)
                {
                    maxnumwords = DefaultMaxNumWords;
                }

                #endregion

                Dictionary<string, string> dictionaryItems = new();

                if (psuedonymn == 0)
                {
                    //get the dictionary   
                    if (!_memoryCache.TryGetValue(BasicDictionaryCacheKey, out dictionaryItems))
                    {
                        //var db = new DictionaryDBContext();
                        dictionaryItems = _db.Dictionary.FromSqlRaw("exec get_basic_english_dictionary").ToDictionary(kvp => kvp.Word, kvp => kvp.Word_ordered_array);

                        //go back to file based


                        _memoryCache.Set(BasicDictionaryCacheKey, dictionaryItems);
                    }
                }
                else if (psuedonymn ==1 )
                {

                    // Read the contents of the CSV files containing male first names, last names, and initials
                    string[] male_first_names = System.IO.File.ReadAllLines(@"C:\SoftbotLabs\SoftbotLabsAnagramAPI\SoftbotLabsAnagramAPI\census\topfemale2016.csv");
                    string[] last_names = System.IO.File.ReadAllLines(@"C:\SoftbotLabs\SoftbotLabsAnagramAPI\SoftbotLabsAnagramAPI\census\lastnames.csv");
                    string[] initials = System.IO.File.ReadAllLines(@"C:\SoftbotLabs\SoftbotLabsAnagramAPI\SoftbotLabsAnagramAPI\census\initials.csv");

                    foreach (string s in male_first_names)
                    {
                        if (!dictionaryItems.ContainsKey(s.ToLower()))
                        {
                            dictionaryItems.Add(s.ToLower(), new string(s.ToLower().OrderBy(c => c).ToArray()).ToLower().Replace("'", "'"));
                        }

                        firstnamestocheck.Add(s.ToLower());
                    }

                    foreach (string s in initials)
                    {
                        if (!dictionaryItems.ContainsKey(s.ToLower()))
                        {
                            dictionaryItems.Add(s.ToLower(), new string(s.ToLower().OrderBy(c => c).ToArray()).ToLower().Replace("'", "'"));
                        }

                        firstnamestocheck.Add(s.ToLower());
                    }

                    foreach (string s in last_names)
                    {

                        string[] lastNameColumns = s.Split(',');
                        if (!dictionaryItems.ContainsKey(lastNameColumns[0].ToLower()))
                        {
                            dictionaryItems.Add(lastNameColumns[0].ToLower(), new string(lastNameColumns[0].ToLower().OrderBy(c => c).ToArray()).ToLower().Replace("'", "'"));
                        }
                    }
                }

                //Get the anagrams and return them as JSON
                AnagramBL anagramBL = new();
                List<string> Output = AnagramBL.GetAnagrams(input.Trim(), minwordlength, maxnumwords, dictionaryItems);

                //do some clean up if psuedonyms, move intial to middle, foramt o', etc.
                if (psuedonymn == 1)
                {
                    List<string> psuedonymn_out = new();
                    foreach (var name in Output)
                    {
                        string[] split_name = name.Split(' ');
                        if (split_name.Length == 2)
                        {
                            if (firstnamestocheck.Contains(split_name[0].ToLower()))
                            {
                                psuedonymn_out.Add(name);
                            }
                        }
                        else if (split_name.Length == 3)
                        {
                            if (split_name[0].Trim().Length == 1 && split_name[1].Length > 1)
                            {
                                if (firstnamestocheck.Contains(split_name[1].ToLower()))
                                {
                                    string newname = string.Empty;
                                    if (split_name[0] == "o")
                                    {
                                        newname = split_name[1] + " " + split_name[0] + "'" + split_name[2];
                                    }
                                    else
                                    {
                                        newname = split_name[1] + " " + split_name[0] + ". " + split_name[2];
                                    }

                                    if (!psuedonymn_out.Contains(newname))
                                        psuedonymn_out.Add(newname);
                                }
                            }
                            if (split_name[0].Length > 1 && split_name[1].Length == 1)
                            {

                                if (firstnamestocheck.Contains(split_name[0].ToLower()))
                                {
                                    string newname = string.Empty;
                                    if (split_name[1] == "o")
                                    {
                                        newname = split_name[0] + " " + split_name[1] + "'" + split_name[2];
                                    }
                                    else
                                    {
                                        newname = split_name[0] + " " + split_name[1] + ". " + split_name[2];
                                    }

                                    if (!psuedonymn_out.Contains(newname))
                                        psuedonymn_out.Add(newname);
                                }
                            }
                        }
                    }
                    psuedonymn_out.Sort();
                    Output = psuedonymn_out;
                }

                sw.Stop();

                // add the elapsed time metrics for the user's consumption
                Output.Add($"Elapsed Time : {sw.ElapsedMilliseconds} ");
                measurements.Add("ElapsedTime", maxnumwords);

                return Output;
            }
            catch (Exception ex)
            {
                //set up something for the call to show failed
                List<string> Error = new()
                {
                    $"error: {ex.Message}"
                };

                return Error;
            }
        }

        [GeneratedRegex("[^a-z]")]
        private static partial Regex MyRegex();
    }
}