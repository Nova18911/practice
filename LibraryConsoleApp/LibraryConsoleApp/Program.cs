using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LibraryConsoleApp.Data;
using LibraryConsoleApp.Services;
using System.Text.Json;

namespace LibraryConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Получаем путь к папке с исполняемым файлом
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var configPath = Path.Combine(basePath, "appsettings.json");

                // Если файл конфигурации не существует, создаем его
                if (!File.Exists(configPath))
                {
                    var defaultConfig = new
                    {
                        ConnectionStrings = new
                        {
                            DefaultConnection = "Server=localhost;Database=LibraryDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
                        }
                    };

                    var json = JsonSerializer.Serialize(defaultConfig, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(configPath, json);
                }

                // Настройка конфигурации
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("Connection string not found in appsettings.json");
                    Console.ReadKey();
                    return;
                }

                // Проверка подключения через ADO.NET
                using (var adoRepo = new AdoNetRepository(connectionString))
                {
                    try
                    {
                        await adoRepo.GetAllAuthorsAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Database connection failed: {ex.Message}");
                        Console.WriteLine("Please check your connection string and database.");
                        Console.ReadKey();
                        return;
                    }
                }

                // Настройка EF
                var services = new ServiceCollection();
                services.AddDbContext<LibraryDbContext>(options =>
                    options.UseSqlServer(connectionString));

                services.AddScoped<EfRepository>();
                services.AddScoped<AdoNetRepository>(provider =>
                    new AdoNetRepository(connectionString));

                var serviceProvider = services.BuildServiceProvider();

                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

                    try
                    {
                        await dbContext.Database.CanConnectAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Database connection failed: {ex.Message}");
                        Console.ReadKey();
                        return;
                    }

                    var adoRepo = scope.ServiceProvider.GetRequiredService<AdoNetRepository>();
                    var efRepo = scope.ServiceProvider.GetRequiredService<EfRepository>();

                    // Запуск меню (без лишних сообщений)
                    var menuService = new MenuService(adoRepo, efRepo, false);
                    await menuService.RunAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                Console.ReadKey();
            }
        }
    }
}