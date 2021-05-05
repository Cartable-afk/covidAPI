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
        private const string VERSION = "1.0";
        private const string GLOBAL_DATA_URL = "https://coronavirusapi-france.vercel.app/FranceLiveGlobalData";
        private const string DEPARTEMENT_DATA_URL = "https://coronavirusapi-france.vercel.app/LiveDataByDepartement?";
        private const string ALL_DEPARTEMENT = "https://coronavirusapi-france.vercel.app/AllLiveData";

        private static HttpClient client;
        private static Printer printer;
        static async Task<string> GetGlobalDataAsync(string requestURL)
        {
            var data = string.Empty;
            var response = await client.GetAsync(requestURL);
            
            if (response.IsSuccessStatusCode)
            {
                data = await response.Content.ReadAsStringAsync();
            }

            return data;
        }

        static void menu()
        {
            var pr = new Printer()
            {
                TableWidth = 105
            };

            pr.Line();
            pr.Row("Commandes", "Description");
            pr.Line();
            pr.Row("global", "Toutes les informations en France");
            pr.Line();
            pr.Row("Departement", "Toutes les informations pour un département");
            pr.Line();
            pr.Row("allData", "Toutes les informations en France par département");
            pr.Line();
            pr.Row("menu", "Affiche le menu des commandes");
            pr.Line();
            pr.Row("exit", "Ferme l'application");
            pr.Line();

        }

        public static void Main(string[] args)
        {
            client = new HttpClient();

            Console.WriteLine("API COVID Version {0}", VERSION);
            menu();

            while (true)
            {
                args = Console.ReadLine().Split(' ');
                var commande = args[0];

                printer = new Printer()
                {
                    TableWidth = 96
                };

                switch (commande)
                {
                    case "global":
                        var json = GetGlobalDataAsync(GLOBAL_DATA_URL).GetAwaiter().GetResult();
                        var data = JObject.Parse(json).SelectToken("FranceGlobalLiveData").ToObject<List<Data>>().First();
                        printer.Line();
                        printer.Row("Date", "Département", "Décès", "Guéris", "Hospitalisé", "Réanimation");
                        printer.Line();
                        printer.Row(data.date.ToString("dd/MM/yyyyy"), data.nom, data.deces.ToString(), data.gueris.ToString(), data.hospitalises.ToString(), data.reanimation.ToString());
                        printer.Line();
                        break;

                    case "departement":

                        Console.WriteLine("Entrez un département : ");

                        var departement = DEPARTEMENT_DATA_URL + "Departement=" + Console.ReadLine();

                        try 
                        {
                            departement.ToString();
                        } 
                        catch (Exception e) 
                        {
                            Console.WriteLine(e);
                            Console.WriteLine("Entrez un nom de département valide (uniquement du texte)");
                        }

                        json = GetGlobalDataAsync(departement).GetAwaiter().GetResult();
                        data = JObject.Parse(json).SelectToken("LiveDataByDepartement").ToObject<List<Data>>().First();

                        printer.Line();
                        printer.Row("Date", "Département", "Décès", "Guéris", "Hospitalisé", "Réanimation");
                        printer.Line();
                        printer.Row(data.date.ToString("dd/MM/yyyy"), data.nom, data.deces.ToString(), data.gueris.ToString(), data.hospitalises.ToString(), data.reanimation.ToString());
                        printer.Line();
                        break;

                    case "allData":
                        json = GetGlobalDataAsync(ALL_DEPARTEMENT).GetAwaiter().GetResult();                                        //Obtention du JSON en brute
                        var datas = JObject.Parse(json).SelectToken("allLiveFranceData").ToObject<List<Data>>().ToArray();          //Deserialisation du JSON pour obtenir un Araway 
                        
                        printer.Line();
                        printer.Row("Date", "Pays", "Décès", "Guéris", "Hospitalisé", "Réanimation");
                        printer.Line();
                        
                        for (int i=0; i < 102; i++)
                        {
                            printer.Row(datas[i].date.ToString("dd/MM/yyyyy"), datas[i].nom, datas[i].deces.ToString(), datas[i].gueris.ToString(), datas[i].hospitalises.ToString(), datas[i].reanimation.ToString());
                            printer.Line();
                        }                       
                        break;

                    case "exit":
                        Environment.Exit(0);
                        break;

                    case "menu":
                        menu();
                        break;

                    default:
                        break;
                }
            }
        }
    }
}