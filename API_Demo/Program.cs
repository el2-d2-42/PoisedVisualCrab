using System;
using Newtonsoft.Json;
using System.Net.Http;

namespace ISSDeserializer
{
    class Program
    {
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

                    Console.WriteLine($"Name: {issData.Name}");
                    Console.WriteLine($"Altitude: {issData.altitude}");
                    Console.WriteLine($"Longitude: {issData.longitude}");
                    Console.WriteLine($"Latitude: {issData.Latitude}");
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
        public string ?Name { get; set; }
        public int Id { get; set; }
        public double Latitude { get; set; }
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
