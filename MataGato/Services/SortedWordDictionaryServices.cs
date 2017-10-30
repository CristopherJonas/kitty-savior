using MataGato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MataGato.Services
{
    public class SortedWordDictionaryServices
    {
        public static List<SorterWord> SaveWordOnMyDictionary(List<SorterWord> savedDictionary, SorterWord returnedWord)
        {
            savedDictionary.Add(returnedWord);
            return savedDictionary;
        }

        public static SorterWord SearchWordOnMyDictionary(List<SorterWord> savedDictionary, string word)
        {
            var item = savedDictionary.FirstOrDefault(o => o.Word == word);
            return item;
        }
    }
}
