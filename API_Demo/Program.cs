using System;
using Newtonsoft.Json;
using System.Net.Http;

namespace ISSDeserializer
{
    class Program
    {
        /*static void Main(string[] args)
        {
            string jsonResponse = @"{
                ""name"": ""iss"",
                ""id"": 25544,
                ""latitude"": 50.11496269845,
                ""longitude"": 118.07900427317,
                ""altitude"": 408.05526028199,
                ""velocity"": 27635.971970874,
                ""visibility"": ""daylight"",
                ""footprint"": 4446.1877699772,
                ""timestamp"": 1364069476,
                ""daynum"": 2456375.3411574,
                ""solar_lat"": 1.3327003598631,
                ""solar_lon"": 238.78610691196,
                ""units"": ""kilometers""
            }";

            ISSData issData = JsonConvert.DeserializeObject<ISSData>(jsonResponse);

            Console.WriteLine($"Name: {issData.name}");
            Console.WriteLine($"ID: {issData.id}");
            // ... (print other properties as needed)
            Console.ReadKey();
        }*/
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string url = "https://api.wheretheiss.at/v1/satellites/25544";
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    ISSData ?issData = JsonConvert.DeserializeObject<ISSData>(jsonResponse);

                    Console.WriteLine($"Name: {issData.name}");
                    Console.WriteLine($"Altitude: {issData.altitude}");
                    Console.WriteLine($"Longitude: {issData.longitude}");
                    Console.WriteLine($"Latitude: {issData.latitude}");
                    // ... (print other properties as needed)
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            Console.ReadKey();
        }
    }

    public class ISSData
    {
        public string ?name { get; set; }
        public int id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public double altitude { get; set; }
        public double velocity { get; set; }
        public string ?visibility { get; set; }
        public double footprint { get; set; }
        public long timestamp { get; set; }
        public double daynum { get; set; }
        public double solar_lat { get; set; }
        public double solar_lon { get; set; }
        public string ?units { get; set; }
    }
}
