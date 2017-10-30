using MataGato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MataGato
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static List<SorterWord> savedDictionary = new List<SorterWord>();
        static List<SorterWord> baseDictionary = new List<SorterWord>();
        static int killedKitten = 0;

      

        static void Main(string[] args)
        {
            initializeBaseDictionary();
            var savedDictionary = new List<SorterWord>();

            client.BaseAddress = new Uri("http://testes.ti.lemaf.ufla.br/api/Dicionario/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            while (true)
            {
                Console.WriteLine("Por favor, digite a palavra e tecle ENTER para iniciar a pesquisa, ou somente ENTER para sair.");
                string word = Console.ReadLine();

                if (!String.IsNullOrEmpty(word))
                {
                    Task<SorterWord> callTask = Task.Run(() => SearchWord(word));
                    callTask.Wait();
                    var astr = callTask.Result;
                    Console.WriteLine("A palavra {0} está na posição {1}. Para encontrarmos a palavra, {2} gatinhos foram mortos. ", word, astr.Position, killedKitten);
                    killedKitten = 0;
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }

        static async Task<SorterWord> GetWordAsync(string position)
        {
            string word = null;
            HttpResponseMessage response = await client.GetAsync(position);
            if (response.IsSuccessStatusCode)
            {
                word = await response.Content.ReadAsStringAsync();
            }
            killedKitten++;
            return new SorterWord(word.Substring(1, word.Length - 2), Convert.ToInt32(position));
        }

        private static SorterWord SearchWordOnMyDictionary(string word)
        {
            var item = savedDictionary.FirstOrDefault(o => o.Word == word);
            return item;
        }

        private static async Task<SorterWord> SearchWord(string word)
        {
            var isWordOnMyDictionary = SearchWordOnMyDictionary(word);
            if (isWordOnMyDictionary != null)
            {
                return isWordOnMyDictionary;
            }
            else
            {
                var currentChar = word[0].ToString();
                // string nextChar = (word[0] == 'z' ? 'a' : (char)((int)word[0] + 1)).ToString();

                int minValue = getMinorIndex(currentChar);

                int maxValue = getMajorIndex(currentChar);

                if (minValue >= maxValue)
                {
                    maxValue = maxValue + 10; //salto de 2000
                }

                return await GetWord(minValue, maxValue, word);
            }
        }

        private static async Task<SorterWord> GetWord(int minorPosition, int majorPosition, string word)
        {
            if (minorPosition > majorPosition)
            {
                return new SorterWord("", -1);
            }
            else
            {
                Random rnd = new Random();
                int next = 0;

                do
                {
                    next = rnd.Next(minorPosition, majorPosition);
                } while (savedDictionary.Any(obj => obj.Position == next));

                var returnedWord = await GetWordAsync(next.ToString());
                saveWordOnMyDictionary(returnedWord);

                if (String.Compare(word, returnedWord.Word) == 0)
                {
                    return returnedWord;
                }
                else
                {
                    return await GetWord(minorPosition, majorPosition, word);
                }
            }
        }

        private static void saveWordOnMyDictionary(SorterWord returnedWord)
        {
            savedDictionary.Add(returnedWord);
        }

        private static void initializeBaseDictionary()
        {
            char baseChar = 'A';
            while (baseChar != 'Z')
            {
                baseDictionary.Add(new SorterWord(baseChar.ToString(), -1));
                baseChar++;
            }
            baseDictionary.Add(new SorterWord("Z", -1));
        }

        private static int getMinorIndex(string indexChar)
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
                return getMinorIndex(baseDictionary[getIndex].Word);
            }
        }

        private static int getMajorIndex(string indexChar)
        {
            var max = baseDictionary.FirstOrDefault(x => x.Word == indexChar.ToString());

            if (max.Position != -1 || max.Word == "Z")
            {
                return max.Position;
            }
            else
            {
                var getIndex = baseDictionary.IndexOf(max) + 1;
                return getMajorIndex(baseDictionary[getIndex].Word);
            }
        }
    }
}