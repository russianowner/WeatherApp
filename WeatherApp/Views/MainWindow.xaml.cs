using System.Windows;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        private WeatherService _weatherService = new();
        private string _currentSource = "auto"; 

        public MainWindow()
        {
            InitializeComponent();
        }
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var city = CityTextBox.Text.Trim();
            if (string.IsNullOrEmpty(city)) return;
            WeatherData? data;
            switch (_currentSource)
            {
                case "openweather":
                    data = await _weatherService.GetFromOpenWeatherOnlyAsync(city);
                    break;
                case "wttr":
                    data = await _weatherService.GetFromWttrOnlyAsync(city);
                    break;
                default:
                    data = await _weatherService.GetWeatherAsync(city);
                    break;
            }
            if (data == null)
            {
                return;
            }
            WeatherText.Text = $"🧐Источник: {data.Source}\n🤓{data.City}, {data.Country}\n😳{data.Temperature}°C\n{data.Description}";
        }
        private void SwitchSourceButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSource == "auto") _currentSource = "openweather";
            else if (_currentSource == "openweather") _currentSource = "wttr";
            else _currentSource = "auto";
            SwitchSourceButton.Content = _currentSource switch
            {
                "auto" => "Источник: авто",
                "openweather" => "Источник: OpenWeather",
                "wttr" => "Источник: wttr.in",
                _ => "Источник: авто"
            };
        }
        private void CityTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (CityTextBox.Text == "Введи название города")
            {
                CityTextBox.Text = "";
                CityTextBox.Foreground = System.Windows.Media.Brushes.DarkBlue;
            }
        }
        private void CityTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                CityTextBox.Text = "Введи название города";
                CityTextBox.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}
