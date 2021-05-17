using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;

namespace SearchSharp
{
    public partial class Search<T>
    {
        private static readonly Regex LetterOrNumCharRegex = new("^[a-zA-Z0-9]+$");

        public Collection<string> GenerateKeyword(char c)
        {
            string cString = c.ToString();

            Collection<string> result = new();

            if (EnableChinesePinyinSearch &&
                ChineseChar.IsValidChar(c))
            {
                // Chinese char

                foreach (string s in new ChineseChar(c).Pinyins
                    //.Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => Regex.Replace(x, @"\d", "").ToLower())
                    .Distinct())
                    result.Add(s); // Pinyin

                foreach (string firstChar in result.Select(x => x.Substring(0, 1)).ToArray())
                    result.Add(firstChar); // 1st char

                result.Add(cString); // Original Chinese char
            }
            else if (LetterOrNumCharRegex.IsMatch(cString))
            {
                result.Add(cString.ToLower());
                // result.Add(cString.ToUpper()); // Search() method uses searchText.ToLower()
            }

            return result;
        }
    }
}
