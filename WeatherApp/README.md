# WeatherApp WPF
---
- Простое WPF приложение для получения погоды. Использует:
---
- OpenWeather API
- wttr.in 
---
- A simple WPF app for getting the weather. Uses:
---
- OpenWeather API
- wttr.in 
---

## Как использовать

1. Копируем репозиторий
2. В appsettings.json вставляем апи токен с https://home.openweathermap.org/api_keys
3. Строим проект и запускаем
4. Вводим город и узнаем погоду
5. Можно переключать источник через кнопку "Сменить источник"

---

## How to use

1. Copy the repository
2. In appsettings.json embed api token with https://home.openweathermap.org/api_keys
3. We build the project and launch
4. Enter the city and find out the weather
5. You can switch the source via the "Change source" button

----

## NuGet Packages:
- dotnet add package Microsoft.Extensions.Configuration
- dotnet add package Microsoft.Extensions.Configuration.Json
- dotnet add package Microsoft.Extensions.Configuration.FileExtensions
- System.Net.Http
- System.Text.Json

---

## Архитектура
1. Configuration/
- AppConfig.cs:
- Центральное место для получения настроек приложения.
- Читает appsettings.json с ключом OpenWeather и базовым URL.
- Служит как единообразный доступ к конфигу для всех сервисов
- Используется в WeatherService для получения _apiKey и _baseUrl
---
2. Models/
- WeatherData.cs:
- Содержит город, страну, температуру, описание погоды и источник (Source)
- Используется всеми сервисами для унифицированного возврата данных
---
3. Services/
1. WeatherService.cs:
- Основной сервис для получения погоды
- Сначала пробует OpenWeather, если неудачно — WttrService
- Поддерживает ручной выбор источника (GetFromOpenWeatherOnlyAsync, GetFromWttrOnlyAsync)
---
2. WttrService.cs:
- Получает данные с wttr.in через JSON API
- Парсит JSON и возвращает WeatherData
- Работает без API ключа, используется как fallback
---
3. WeatherFallbackService.cs:
- Парсит погоду с Google через HTML и Regex (как запасной вариант)
- Сейчас не используется напрямую, но можно добавить как третий источник
---
4. Views/
- MainWindow.xaml - UI
- MainWindow.xaml.cs:
- Получение города из TextBox
- Вызов методов WeatherService
- Обновление TextBlock с результатами
- Переключение источника
----

- appsettings.json - содержит API ключ OpenWeather и базовый URL

---

## Architecture
1. Configuration/
- AppConfig.cs:
- The central place to get the application settings.
- Reads appsettings.json with the OpenWeather key and the base URL.
- Serves as a uniform access to the configuration for all services
- Used in WeatherService to get _apiKey and _baseUrl
---
2. Models/
- WeatherData.cs:
- Contains city, country, temperature, weather description and source
- Used by all services for unified data return
---
3. Services/
1. WeatherService.cs:
- The main service for getting the weather
- Tries OpenWeather first, if unsuccessful — WttrService
- Supports manual source selection (GetFromOpenWeatherOnlyAsync, GetFromWttrOnlyAsync)
---
2. WttrService.cs:
- Receives data from wttr.in via the JSON API
- Parses JSON and returns WeatherData
- Works without an API key, used as a fallback
---
3. WeatherFallbackService.cs:
- Parses the weather with Google via HTML and Regex (as a backup option)
- Currently not used directly, but can be added as a third source.
---
4. Views/
- MainWindow.xaml - UI
- MainWindow.xaml.cs:
- Getting a city from a TextBox
- Calling the WeatherService methods
- Updating the TextBlock with the results
- Switching the source
----

- appsettings.json - contains the OpenWeather API key and the base URL

---