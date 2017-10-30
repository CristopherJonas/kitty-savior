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
        //static HttpClient client = new HttpClient();

        //static string SortNewWord(int lower, int higher)
        //{
        //    return "dfdsf";
        //}

        static HttpClient client = new HttpClient();

        public static async Task<string> GetWordAsync(string position)
        {
            string word = null;
            HttpResponseMessage response = await client.GetAsync(position);
            if (response.IsSuccessStatusCode)
            {
                word = await response.Content.ReadAsStringAsync();
            }
            return word;
        }

        public static async Task RunAsync(string val)
        {
            client.BaseAddress = new Uri("http://testes.ti.lemaf.ufla.br/api/Dicionario/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                // Get the product
                string name = await GetWordAsync(val);
                Console.WriteLine(name);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
