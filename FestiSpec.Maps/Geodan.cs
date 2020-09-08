using System;
using System.Net;
using System.Net.Http;
using System.Windows;
using FestiSpec.Maps.Interfaces;

namespace FestiSpec.Maps
{
    public class Geodan : IGeodan
    {
        private readonly string _key;

        public Geodan(string key)
        {
            _key = key;
        }

        public void ShowAdresses(int zoom, double x, double y)
        {
            var url = $"addresses/wmts?layer=addressesbuildings&request=GetTile&service=WMTS&version=1.0.0&format=image%2Fpng&tilematrixset=EPSG%3A28992&tilematrix=00&tilecol=0&tilerow=0&style=default&servicekey={_key}";
            Console.WriteLine("Making API Call...");
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("https://services.geodan.nl/data/geodan/gws/nld/");
                var response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("Result: " + result);
            }
            Console.ReadLine();
        }

        public string FindAdress(string query)
        {
            try
            {
                var url = $"https://services.geodan.nl/geosearch/free?q={query}&q.op=AND&servicekey={_key}";

                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetStringAsync(url).Result;
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    return json.response.docs[0].geom;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Locatie niet gevonden, controlleer of de locatie van het festival correct is", "Locatie niet gevonden");
                return "";
            }
        }

        public dynamic FindRoute(string query)
        {
            try
            {
                var url = $"https://services.geodan.nl/routing/addressroute?{query}&servicekey={_key}";

                using (var httpClient = new HttpClient())
                {
                    var response = httpClient.GetStringAsync(url).Result;
                    dynamic json = Newtonsoft.Json.JsonConvert.DeserializeObject(response);
                    return json;
                }
            }
            catch (Exception)
            {
                throw new NoRouteException();
            }
        }
    }
}
