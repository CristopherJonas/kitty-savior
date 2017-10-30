using MataGato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MataGato.Services
{
    public class SortService
    {
        public static async Task<SorterWord> SearchWord(HttpClient client, List<SorterWord> savedDictionary, List<SorterWord> baseDictionary, int jump, string word)
        {
            var isWordOnMyDictionary = SortedWordDictionaryServices.SearchWordOnMyDictionary(savedDictionary, word);
            if (isWordOnMyDictionary != null)
            {
                return isWordOnMyDictionary;
            }
            else
            {
                var currentChar = word[0].ToString();

                int minValue = BaseDictionaryService.GetMinorIndex(baseDictionary, currentChar);
                int maxValue = BaseDictionaryService.GetMajorIndex(baseDictionary, currentChar);

                if (minValue >= maxValue)
                {
                    maxValue = maxValue + jump;
                }

                return await GetWord(client, savedDictionary, jump, minValue, maxValue, word);
            }
        }

        public static async Task<SorterWord> GetWord(HttpClient client, List<SorterWord> savedDictionary, int jump, int minorPosition, int majorPosition, string word)
        {
            if (minorPosition > majorPosition)
            {
                return new SorterWord("", -1);
            }
            else
            {
                Random rnd = new Random();
                int next = 0;

                int maxNumOfPositionsInInterval = majorPosition - minorPosition;
                var savedPositionsOnDictionary = savedDictionary.Count(x => x.Position >= minorPosition && x.Position <= majorPosition);

                if (savedPositionsOnDictionary != maxNumOfPositionsInInterval)
                {
                    try
                    {
                        do
                        {
                            next = rnd.Next(minorPosition, majorPosition);
                        } while (savedDictionary.Any(obj => obj.Position == next));

                        var returnedWord = await ConnService.GetWordAsync(next.ToString(), client);
                        KilledKittens.KillCat();
                        savedDictionary = SortedWordDictionaryServices.SaveWordOnMyDictionary(savedDictionary, returnedWord);

                        if (String.Compare(word, returnedWord.Word) == 0)
                        {
                            return returnedWord;
                        }
                        else
                        {
                            return await GetWord(client, savedDictionary, jump, minorPosition, majorPosition, word);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return null;
                    }
                }
                else
                {
                    return await GetWord(client, savedDictionary, jump, majorPosition, majorPosition + jump, word);
                }


            }
        }

    }
}
