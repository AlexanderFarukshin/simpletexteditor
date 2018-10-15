using System.Collections.Generic;
using System.Linq;
using Server.DataBase;
using Server.DataBase.Models;

namespace Server.Dictionary
{
    public class DictionaryManager : IDictionaryManager
    {
        private readonly ApplicationDbContext _context;

        public DictionaryManager()
        {
            _context = new ApplicationDbContext();
        }

        public void Create(Dictionary<string, int> frequentWords)
        {
            Clear();
            Update(frequentWords);
        }

        public void Update(Dictionary<string, int> frequentWords)
        {
            foreach (var word in frequentWords)
            {
                var existingWord = _context.Words.FirstOrDefault(x => x.Word == word.Key);
                if (existingWord != null) existingWord.Frequency += word.Value;
                else _context.Words.Add(new WordInfo { Word = word.Key, Frequency = word.Value });
            }

            _context.SaveChanges();
        }

        public void Clear()
        {
            _context.Words.RemoveRange(_context.Words);
            _context.SaveChanges();
        }

        public string[] GetSuitableWords(string prefix, int maxCount)
        {
            return
                _context.Words
                .Where(x => x.Word.StartsWith(prefix))
                .OrderByDescending(x => x.Frequency)
                .ThenBy(x => x.Word)
                .Select(x => x.Word)
                .Take(maxCount)
                .ToArray();
        }
    }
}
