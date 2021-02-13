using System;
using System.Text;

namespace Polyalphabetic.Library
{
    #region Algorithm
    /*
     * Step1:
     * 
     * Step2:
     * 
     * Step3:
     * 
    */
    #endregion

    /// <summary>
    /// Simple implementation of polyalphabetic cipher
    /// </summary>
    public class SimplePolyalphabetic
    {
        /// <summary>
        /// Alphabet 
        /// </summary>
        private readonly char[] _characters = new char[] 
        {
            ' ', '?', '!',
            '1', '2', '3', '4', '5', '6', '7', '8', '9', '0',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L',
            'M', 'N', 'O', 'P', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };

        /// <summary>
        /// Count of alphabet
        /// </summary>
        public int AlphabetCount => _characters.Length;


        /// <summary>
        /// Method to encode input string by key
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string Encode(string input, string keyword)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int keywordIndex = 0;

            input = input.ToUpper();
            keyword = keyword.ToUpper();

            foreach (char symbol in input)
            {
                int index = (Array.IndexOf(_characters, symbol) + Array.IndexOf(_characters, keyword[keywordIndex])) 
                        % AlphabetCount;

                stringBuilder.Append(_characters[index]);

                keywordIndex++;
                keywordIndex = ResetKeywordIndex(keyword, keywordIndex);
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        ///  Method to dencode input string by key
        /// </summary>
        /// <param name="input"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public string Decode(string input, string keyword)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int keywordIndex = 0;

            input = input.ToUpper();
            keyword = keyword.ToUpper();


            foreach (char symbol in input)
            {
                int index = (Array.IndexOf(_characters, symbol) + AlphabetCount -
                         Array.IndexOf(_characters, keyword[keywordIndex])) % AlphabetCount;

                stringBuilder.Append(_characters[index]);

                keywordIndex++;
                keywordIndex = ResetKeywordIndex(keyword, keywordIndex);
            }

            return stringBuilder.ToString();
        }

        private int ResetKeywordIndex(string keyword, int keywordIndex)
        {
            if (keywordIndex + 1 == keyword.Length)
                keywordIndex = 0;

            return keywordIndex;
        }
    }
}
