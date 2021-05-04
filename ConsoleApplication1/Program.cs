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
        private const string VERSION = "0.1";
        private const string GLOBAL_DATA_URL = "https://coronavirusapi-france.now.sh/FranceLiveGlobalData";
        private const string DEPARTEMENT_DATA_URL = "https://coronavirusapi-france.now.sh/LiveDataByDepartement?";
        private const string ALL_DEPARTEMENT = "https://coronavirusapi-france.now.sh/AllLiveData";

       
        
        public static void Main(string[] args)
        {
            Console.WriteLine("API COVID Version {0}", VERSION);

            while (true)
            {
                args = Console.ReadLine().Split(' ');
                var commande = args[0];

                switch (commande)
                {
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