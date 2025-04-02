using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnagramAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class AIModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AIModel"/> class.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <param name="minWordLength">Minimum length of the word.</param>
        /// <param name="maxNumWords">The maximum number words.</param>
        /// <param name="elapsedTime">The elapsed time.</param>
        public AIModel(string word, int minWordLength, int maxNumWords, int elapsedTime)
        {
            Word = word;
            MinWordLength = minWordLength;
            MaxNumWords = maxNumWords;
            ElapsedTime = elapsedTime;
        }

        /// <summary>
        /// Gets or sets the word.
        /// </summary>
        /// <value>
        /// The word.
        /// </value>
        public string Word { get; set; }

        /// <summary>
        /// Gets or sets the minimum length of the word.
        /// </summary>
        /// <value>
        /// The minimum length of the word.
        /// </value>
        public int MinWordLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum number words.
        /// </summary>
        /// <value>
        /// The maximum number words.
        /// </value>
        public int MaxNumWords { get; set; }

        /// <summary>
        /// Gets or sets the elapsed time.
        /// </summary>
        /// <value>
        /// The elapsed time.
        /// </value>
        public int ElapsedTime { get; set; }
    }
}
