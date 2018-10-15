using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;


namespace Server.TextAnalyzer
{
    public static class TextFileAnalyzer
    {
        public static Dictionary<string, int> GetFrequentWords(string pathToFile, int minFrequency, int minWordLength, int maxWordLength)
        {
            var fileText = ReadTextFromFile(pathToFile);
            fileText = ReplaceLineBreaksToSpace(fileText);

            return
               fileText
               .Split(' ')
               .Select(x => RemoveNotTextSymbols(x).ToLower())
               .Where(x => x.Length >= minWordLength && x.Length <= maxWordLength)
               .GroupBy(x => x)
               .Where(x => x.Count() >= minFrequency)
               .OrderByDescending(x => x.Count())
               .ToDictionary(x => x.Key, x => x.Count());
        }

        private static string ReadTextFromFile(string pathToFile)
        {
            string fileText;
         
            using (var fileStream = new FileStream(pathToFile, FileMode.Open))
            using (var streamReader = new StreamReader(fileStream))
            {
                fileText = streamReader.ReadToEnd();
            }         

            return fileText;
        }

        private static string ReplaceLineBreaksToSpace(string text)
        {
            return Regex.Replace(text, "\r\n", " ");
        }

        private static string RemoveNotTextSymbols(string word)
        {
            return Regex.Replace(word, @"\W", "");
        }        
    }
}
