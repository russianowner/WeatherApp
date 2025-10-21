using System.Net.Http;
using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient = new();
        private readonly string _apiKey;
        private readonly string _baseUrl;
        private readonly WttrService _wttrService = new();

        public WeatherService()
        {
            _apiKey = Configuration.AppConfig.Get("OpenWeather:ApiKey");
            _baseUrl = Configuration.AppConfig.Get("OpenWeather:BaseUrl");
        }
        public async Task<WeatherData?> GetWeatherAsync(string city)
        {
            var data = await TryOpenWeatherAsync(city);
            if (data != null)
            {
                data.Source = "OpenWeather";
                return data;
            }
            var wttrData = await _wttrService.GetWeatherAsync(city);
            if (wttrData != null)
            {
                wttrData.Source = "wttr.in";
                return wttrData;
            }
            return new WeatherData
            {
                City = city,
                Country = "—",
                Temperature = 0,
            };
        }
        public async Task<WeatherData?> GetFromOpenWeatherOnlyAsync(string city)
        {
            var data = await TryOpenWeatherAsync(city);
            if (data != null) data.Source = "OpenWeather";
            return data;
        }
        public async Task<WeatherData?> GetFromWttrOnlyAsync(string city)
        {
            var data = await _wttrService.GetWeatherAsync(city);
            if (data != null) data.Source = "wttr.in";
            return data;
        }
        private async Task<WeatherData?> TryOpenWeatherAsync(string city)
        {
            try
            {
                var url = $"{_baseUrl}weather?q={city}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }
                if (!json.TrimStart().StartsWith("{"))
                {
                    return null;
                }
                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                return new WeatherData
                {
                    City = city,
                    Country = root.GetProperty("sys").GetProperty("country").GetString(),
                    Temperature = root.GetProperty("main").GetProperty("temp").GetDecimal(),
                    Description = root.GetProperty("weather")[0].GetProperty("description").GetString()
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
