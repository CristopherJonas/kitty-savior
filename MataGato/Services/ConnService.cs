using MataGato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MataGato.Services
{
    public class ConnService
    {
        public static async Task<SorterWord> GetWordAsync(string position, HttpClient client)
        {
            string word = null;
            HttpResponseMessage response = await client.GetAsync(position);
            if (response.IsSuccessStatusCode)
            {
                word = await response.Content.ReadAsStringAsync();
            }
            else
            {
                throw new Exception("Palavra não encontrada");
            }
            return new SorterWord(word.Substring(1, word.Length - 2), Convert.ToInt32(position));
        }
    }
}
