using MataGato.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MataGato.Services;

namespace MataGato
{
    class Program
    {
        static HttpClient client = new HttpClient();
        static List<SorterWord> savedDictionary = new List<SorterWord>();
        static List<SorterWord> baseDictionary = new List<SorterWord>();
        static int killedKitten = 0;
        static int jump = 2000;  // tamanho do salto na amostra 



        static void Main(string[] args)
        {
            baseDictionary = BaseDictionaryService.InitializeBaseDictionary(baseDictionary);
            var savedDictionary = new List<SorterWord>();

            client.BaseAddress = new Uri("http://testes.ti.lemaf.ufla.br/api/Dicionario/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            while (true)
            {
                Console.WriteLine("Por favor, digite a palavra e tecle ENTER para iniciar a pesquisa" + "\r\n" + "Ou somente ENTER para sair.");
                string word = Console.ReadLine();

                if (!String.IsNullOrEmpty(word))
                {
                    Task<SorterWord> callTask = Task.Run(() => SortService.SearchWord(client, savedDictionary, baseDictionary, jump, word));
                    callTask.Wait();
                    var astr = callTask.Result;
                    killedKitten = KilledKittens.ReturnKilledkitten();
                    if (astr != null)
                    {
                        Console.WriteLine("A palavra {0} está na posição {1}. Para encontrarmos a palavra, {2} gatinhos foram mortos." + "\r\n\r\n", word, astr.Position, killedKitten);
                    }
                    KilledKittens.ResetCounter();
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}