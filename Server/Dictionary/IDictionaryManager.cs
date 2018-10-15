using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Dictionary
{
    public interface IDictionaryManager
    {
        void Create(Dictionary<string, int> frequentWords);

        void Update(Dictionary<string, int> frequentWords);

        void Clear();

        string[] GetSuitableWords(string prefix, int maxCount);
    }
}
