using desafioVisualFormaBackEnd.Services;

//Entry point, é aqui que o programa começa
class Program
{
    static async Task Main(string[] args) //Funçao principal ig
    {
        var weatherService = new WeatherService();
        while (true) //Loop para poder usar varias vezes o programa
        {
            //Prompt que vai perguntar ao user pelo nome da cidade, em seguida guarda dentro da var cityName
            Console.WriteLine();
            Console.Write("Digite o nome da cidade (ou 'help' para listar comandos): "); //PS1 PROMPT
            var cityName = Console.ReadLine(); //Ler a line que o user escrever e armazena dentro da var cityName
            Console.WriteLine();

            //Vai verificar se o input esta vazio
            if(string.IsNullOrWhiteSpace(cityName)) //Adoro as funcoes super descritivas do C#
            {
                Console.WriteLine("Por favor, digite um nome de cidade válido!");
                continue;
            }

            //Se o comando escrito pelo user for "exit", entao o programa fecha
            if (cityName
                .Trim()
                .ToLowerInvariant() == "exit") break; //Se detectar que foi escrito "exit", breakar o loop / fechar o programa

            //Para a lista de comandos
            if(cityName
                .Trim()
                .ToLowerInvariant() == "help")
            {
                Console.Clear();
                Console.WriteLine();
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("========== Help ==========");
                Console.ResetColor();
                Console.WriteLine();
                Console.WriteLine("Type: [exit] to close the program.");
                Console.WriteLine("Type: [listCache] to see cities in cache.");
                Console.WriteLine("Type: [clearCache] to clear cities in cache.");
                Console.WriteLine("Type: [city name] to get weather for that city.");
                Console.WriteLine();
                Console.WriteLine("==========================");
                continue;
            }

            //Limpar a cache
            if(cityName.Equals("clearCache", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear();
                weatherService.ClearCache();
                continue;
            }

            //Mostrar cidades em cache
            if (cityName.Equals("listCache", StringComparison.OrdinalIgnoreCase))
            {
                var cachedCities = weatherService.GetCachedCities();
                if (cachedCities.Count == 0)
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("========== Empty cache ==========");
                    Console.ResetColor();
                    Console.WriteLine();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine();
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("========== Cidades em Cache ==========");
                    Console.ResetColor();
                    Console.WriteLine();
                    foreach (var city in cachedCities)
                    {
                        Console.WriteLine("- " + city);
                    }
                    Console.WriteLine();
                    Console.WriteLine("======================================");
                }
                continue;
            }

            //Chamar o serviço para obter a previsao, pega na var cityName, faz um fetch ao serviço weatherService, chamando a funçao GetWeatherAsync com esse nome, e guarda o resultado dentro da var weather
            var weather = await weatherService.GetWeatherAsync(cityName); //Chamar a funçao no weatherService com o nome da cidade

            // Se o resultado do await que a var weather vai receber, chegar empty, pede again o nome da cidade
            if (weather == null)
                continue; //Se a cidade nao foi encontrada, pedir nome da cidade again

            //Se tudo correr fixe, mostrar o result
            Console.Clear();
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("========== Resultado ==========");
            Console.ResetColor();
            Console.WriteLine($"Cidade: {cityName}" + (weather.FromCache ? " (cache)" : ""));
            Console.WriteLine($"Temperatura atual: {weather.Temperature}ºC");
            Console.WriteLine($"Condição: {weather.Condition}");
            if(weather.WindSpeed.HasValue)
            {
                Console.WriteLine($"Velocidade do vento: {weather.WindSpeed} Km/h");
            } else
            {
                Console.WriteLine();
            }
            Console.WriteLine("======== End Resultado ========");
            Console.WriteLine();


        }
    }
}