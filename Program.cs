using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TopLibraryManager.Services;
using TopLibraryManager.Data;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);
    })
    .ConfigureServices((context, services) =>
    {
        // настраиваем контекст базы данных библиотеки
        services.AddDbContext<LibraryDbContext>(options =>
        {
            var connectionString = context.Configuration.GetConnectionString("Library");
            options.UseSqlite(connectionString);
        });

        // регистрируем сервисы
        services.AddTransient<IConsoleUIService, ConsoleUIService>();
        services.AddTransient<ICommandProcessorService, CommandProcessorService>();

        // регистрируем цикл приложения
        services.AddHostedService<AppHostedService>();
    })
    .Build();

await host.RunAsync();
