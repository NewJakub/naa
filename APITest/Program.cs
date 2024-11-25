using System.Text.Json;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace NASA_API_App
{ 
    public class Asteroids 
    {
        [JsonPropertyName("title")]
        public int Id { get; set; }
        public string Name { get; set; }
        public double EstimatedDiameter { get; set; }
        public double DistanceFromEarth { get; set; }
        public bool PotentiallyHazardous { get; set; }

        public string PotentiallyHazardousTranslated => PotentiallyHazardous ? "Ano" : "Ne";
        public string ApproachDate { get; set; }
        public string OrbitingBody { get; set; }

       

        
    }
    public class APIHandler
    {
        private readonly HttpClient _httpClient;

        public APIHandler()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Asteroids>> GetAsteroidsAsync(string startDate, string endDate, string apiKey)
        {
            var url = $"https://api.nasa.gov/neo/rest/v1/feed?start_date={startDate}&end_date={endDate}&api_key={apiKey}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonDocument.Parse(json);

                List<Asteroids> asteroids = data.RootElement
                    .GetProperty("near_earth_objects")
                    .EnumerateObject()
                    .SelectMany(day => day.Value.EnumerateArray())
                    .Select(neo => new Asteroids
                    {
                        Name = neo.GetProperty("name").GetString(),
                        Id = int.Parse(neo.GetProperty("id").GetString()),
                        EstimatedDiameter = GetDoubleValue(neo.GetProperty("estimated_diameter").GetProperty("kilometers").GetProperty("estimated_diameter_max")),
                        DistanceFromEarth = GetDoubleValue(neo.GetProperty("close_approach_data")[0].GetProperty("miss_distance").GetProperty("kilometers")),
                        PotentiallyHazardous = neo.GetProperty("is_potentially_hazardous_asteroid").GetBoolean(),
                       
                    })
                    .ToList();

                return asteroids;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Request Failed - Status Code: {response.StatusCode}");
                Console.WriteLine($"Error Content: {errorContent}");
                throw new Exception($"API request failed with status: {response.StatusCode}");
            }


        }

        private double GetDoubleValue(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Number)
            {
                return element.GetDouble();
            }
            else if (element.ValueKind == JsonValueKind.String && double.TryParse(element.GetString(), out double result))
            {
                return result;
            }
            else
            {
                throw new Exception("Invalid data type for numeric value");
            }
        }
        
        
    }

    public class Program
    {
        static void Main(string[] args)
        {
            ObservableCollection<Asteroids> AsteroidList = new ObservableCollection<Asteroids>();
            CallData();
            Console.ReadLine();
            
        }

        public static async void CallData()
        {
            APIHandler NasaApiHandler = new APIHandler();
            var data = await NasaApiHandler.GetAsteroidsAsync(DateTime.UtcNow.ToString("yyyy-MM-dd"), DateTime.UtcNow.AddDays(2).ToString("yyyy-MM-dd"), "PlgKdthvA7rmL9RpBz2i91Y6Nfy9A5LqtDoh3eKt");
            foreach (Asteroids asteroid in data)
            {
                Console.WriteLine(asteroid.Name);
            }
        }
    }
}
