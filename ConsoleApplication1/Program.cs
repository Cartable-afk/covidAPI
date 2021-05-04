using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        private const string VERSION = "0.11";
        private const string GLOBAL_DATA_URL = "https://coronavirusapi-france.vercel.app/FranceLiveGlobalData";
        private const string DEPARTEMENT_DATA_URL = "https://coronavirusapi-france.app/LiveDataByDepartement?";
        private const string ALL_DEPARTEMENT = "https://coronavirusapi-france.app/AllLiveData";

        private static HttpClient client;
        
        static async Task<string> GetGlobalDataAsync()
        {
            var data = string.Empty;
            var response = await client.GetAsync(GLOBAL_DATA_URL);
            
            if (true)
            {
                data = await response.Content.ReadAsStringAsync();
            }

            return data;
        }

        public static void Main(string[] args)
        {
            client = new HttpClient();

            Console.WriteLine("API COVID Version {0}", VERSION);

            while (true)
            {
                args = Console.ReadLine().Split(' ');
                var commande = args[0];

                switch (commande)
                {
                    case "global":
                        var result = GetGlobalDataAsync().GetAwaiter().GetResult();
                        Console.WriteLine(result);
                        break;

                    case "exit":
                        Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}