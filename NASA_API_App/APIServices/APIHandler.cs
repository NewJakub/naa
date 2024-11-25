using System.Text.Json;
using System.Diagnostics;
using System.IO;
using NASA_API_App.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace NASA_API_App.APIServices
{
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

                var asteroids = data.RootElement
                    .GetProperty("near_earth_objects")
                    .EnumerateObject()
                    .SelectMany(day => day.Value.EnumerateArray())
                    .Select(neo => new Asteroids
                    {
                        Name = neo.GetProperty("name").GetString(),
                        Id = int.Parse(neo.GetProperty("id").GetString()),
                        EstimatedDiameter = GetDoubleValue(neo.GetProperty("estimated_diameter").GetProperty("kilometers").GetProperty("estimated_diameter_max")),
                        Speed = GetDoubleValue(neo.GetProperty("close_approach_data")[0].GetProperty("relative_velocity").GetProperty("kilometers_per_hour")),
                        PotentiallyHazardous = neo.GetProperty("is_potentially_hazardous_asteroid").GetBoolean(),
                        TimeAroundEarth = Math.Round(40.075 / GetDoubleValue(neo.GetProperty("close_approach_data")[0].GetProperty("relative_velocity").GetProperty("kilometers_per_hour")) * 3600),
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
}
