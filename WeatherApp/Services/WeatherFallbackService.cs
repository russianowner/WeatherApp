using System.Net.Http;
using System.Text.RegularExpressions;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherFallbackService
    {
        private readonly HttpClient _httpClient = new();

        public async Task<WeatherData?> GetWeatherFromGoogleAsync(string city)
        {
            try
            {
                var url = $"https://www.google.com/search?q=погода+{Uri.EscapeDataString(city)}";
                _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
                var html = await _httpClient.GetStringAsync(url);
                var match = Regex.Match(html, @"([-+]?\d+)\s*°");
                if (match.Success)
                {
                    var temp = decimal.Parse(match.Groups[1].Value);
                    return new WeatherData
                    {
                        City = city,
                        Country = "—",
                        Temperature = temp,
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
