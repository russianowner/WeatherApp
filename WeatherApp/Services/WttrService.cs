using System.Net.Http;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WttrService
    {
        private readonly HttpClient _httpClient = new();

        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            try
            {
                var url = $"https://wttr.in/{Uri.EscapeDataString(city)}?format=j1";
                var json = await _httpClient.GetStringAsync(url);
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                var current = root.GetProperty("current_condition")[0];
                var area = root.GetProperty("nearest_area")[0];
                var tempStr = current.GetProperty("temp_C").GetString();
                var desc = current.GetProperty("weatherDesc")[0].GetProperty("value").GetString();
                var cityName = area.GetProperty("areaName")[0].GetProperty("value").GetString();
                var country = area.GetProperty("country")[0].GetProperty("value").GetString();
                decimal temperature = 0;
                if (!string.IsNullOrWhiteSpace(tempStr))
                    decimal.TryParse(tempStr, out temperature);
                return new WeatherData
                {
                    City = cityName ?? city,
                    Country = country ?? "—",
                    Temperature = temperature,
                    Description = desc 
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }
    }
}
