using desafioVisualFormaBackEnd.Models;
using System.Net.Http.Json;

//Assim que o Program.cs, chega na parte de guardar o conteudo dentro da var weather, chegamos aqui!
namespace desafioVisualFormaBackEnd.Services
{
    public class WeatherService : IWeatherService //IWeatherService é a Interface da nossa class WeatherService (preciso estudar mais sobre isto)
    {
        private readonly HttpClient _http; //Cliente HTTP para fazer requests
        private readonly Dictionary<string, WeatherResult> _cache; // Cache simples

        public WeatherService()
        {
            _http = new HttpClient();
            _cache = new Dictionary<string, WeatherResult>();
        }

        public void ClearCache() //Função para depois dar para dar clean a cache
        {
            _cache.Clear();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("========== Cache cleared ==========");
            Console.ResetColor();
        }

        public List<string> GetCachedCities() //Funçao para mostrar a lista de cidades ja em cache
        {
            Console.ResetColor();
            return _cache.Keys.ToList();
        }
        
        public async Task<WeatherResult?> GetWeatherAsync(string cityName) //Aqui vai chegar o nome da cidade
        {
            //Normalizar o nome da cidade para usar como Key
            var key = cityName //Aqui vai pegar no nome da cidade, cortar o inicio e o fim por espaços em branco, e por tudo em pequeno, para evitar variaçoes na cache!
                .Trim()
                .ToLowerInvariant();

            //Verificar se a cidade ja esta no cache
            if(_cache.ContainsKey(key))
            {
                var cachedResult = _cache[key]; //Vai buscar o resultado armazenado
                cachedResult.FromCache = true; //Dizer que veio da cache
                return cachedResult; //Se houver resultados na cache, retorna
            }

            try
            {
                // ========== GeoCoding ==========
                //URL do API geocoding
                var geoUrl = $"https://geocoding-api.open-meteo.com/v1/search?name={Uri.EscapeDataString(cityName)}"; //Vai a API buscar o geolocalizacao com query o nome da cidade que chegou aqui
               
                //Fazer request / Tive problemas aqui! TODO: estudar bem esta parte depois
                Console.WriteLine($"Fecthing {geoUrl} ...");
                var geoJson = await _http.GetStringAsync(geoUrl); // recebe como string
                var geoResponse = System.Text.Json.JsonSerializer.Deserialize<GeoResponse>(geoJson);
                Console.WriteLine($"Fecthing complete");

                if (geoResponse?.Results == null || geoResponse.Results.Count == 0)
                {
                    Console.WriteLine("Não foi possível encontrar a cidade. Tente novamente.");
                    return null;
                }

                //Pega o primeiro resultado
                var city = geoResponse.Results[0];

                // ========== OpenWeather ==========
                // Open Weather API
                var weatherUrl = $"https://api.open-meteo.com/v1/forecast?latitude={city.Latitude}&longitude={city.Longitude}&current_weather=true";
                var weatherResponse = await _http.GetFromJsonAsync<WeatherApiResponse>(weatherUrl); //Vai fazer um resquest

                if(weatherResponse?.CurrentWeather ==  null) //Verifica se a resposta ao request deu errado, se sim, mostrar erro
                {
                    Console.WriteLine("Não foi possivel obter o estado do tempo.");
                    return null;
                }

                var result = new WeatherResult //Vai construir um result
                {
                    Temperature = weatherResponse.CurrentWeather.Temperature,
                    WindSpeed = weatherResponse.CurrentWeather.WindSpeed,
                    Condition = MapWeatherCode((int)weatherResponse.CurrentWeather.WeatherCode),
                    FromCache = false
                };

                //Guardar na cache
                _cache[key] = result;

                //Retornar o resultado
                return result;
            }
            catch (Exception ex) //O catch serve caso o Try falhe, basicamente um else
            {
                Console.WriteLine("Erro ao consultar a API de geocoding: " + ex.Message);
                return null;
            }
        }

        // Metodo privado para transformar weatherCode em texto normal ex: 0 -> ceu limpo
        private string MapWeatherCode(int code)
        {
            return code switch
            {
                0 => "Clear sky",
                1 => "Mainly clear",
                2 => "Partly cloudy",
                3 => "Overcast",
                45 => "Fog",
                48 => "Depositing rime fog",
                51 => "Drizzle: Light",
                53 => "Drizzle: Moderate",
                55 => "Drizzle: Dense",
                61 => "Rain: Slight",
                63 => "Rain: Moderate",
                65 => "Rain: Heavy",
                71 => "Snow: Slight",
                73 => "Snow: Moderate",
                75 => "Snow: Heavy",
                80 => "Rain showers: Slight",
                81 => "Rain showers: Moderate",
                82 => "Rain showers: Violent",
                _ => "Unknown"
            };
        }
    }
}
