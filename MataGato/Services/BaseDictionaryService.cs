using MataGato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MataGato.Services
{
    public class BaseDictionaryService
    {
        public static List<SorterWord> InitializeBaseDictionary(List<SorterWord> baseDictionary)
        {
            char baseChar = 'A';
            while (baseChar != 'Z')
            {
                baseDictionary.Add(new SorterWord(baseChar.ToString(), -1));
                baseChar++;
            }
            baseDictionary.Add(new SorterWord("Z", -1));
            return baseDictionary;
        }

        public static int GetMinorIndex(List<SorterWord> baseDictionary, string indexChar)
        {
            if (indexChar == "A")
            {
                return 0;
            }

            var min = baseDictionary.FirstOrDefault(x => x.Word == indexChar.ToString());
            if (min.Position != -1)
            {
                return min.Position;
            }
            else
            {
                var getIndex = baseDictionary.IndexOf(min) - 1;
                return GetMinorIndex(baseDictionary, baseDictionary[getIndex].Word);
            }
        }

        public static int GetMajorIndex(List<SorterWord> baseDictionary, string indexChar)
        {
            var max = baseDictionary.FirstOrDefault(x => x.Word == indexChar.ToString());

            if (max.Position != -1 || max.Word == "Z")
            {
                return max.Position;
            }
            else
            {
                var getIndex = baseDictionary.IndexOf(max) + 1;
                return GetMajorIndex(baseDictionary, baseDictionary[getIndex].Word);
            }
        }
    }
}
