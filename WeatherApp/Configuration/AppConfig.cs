using Microsoft.Extensions.Configuration;
using System.IO;

namespace WeatherApp.Configuration
{
    public static class AppConfig
    {
        private static readonly IConfigurationRoot _configuration;

        static AppConfig()
        {
            var basePath = Directory.GetCurrentDirectory();
            if (!File.Exists(Path.Combine(basePath, "appsettings.json")))
            {             
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory) 
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            _configuration = builder.Build();
        }
        public static string Get(string key)
        {
            var value = _configuration[key];
            if (string.IsNullOrEmpty(value))
            value = value?.Trim().Trim('"');
            return value ?? string.Empty;
        }
    }
}
