using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using TopLibraryManager.Data;
using static System.Formats.Asn1.AsnWriter;

namespace TopLibraryManager.Services;

public class AppHostedService(
    LibraryDbContext libraryDbContext,
    IHostApplicationLifetime appLifetime
) : BackgroundService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;

    /// <summary>
    /// Менеджер жизненного цикла приложения
    /// </summary>
    private readonly IHostApplicationLifetime _appLifetime = appLifetime;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // создаем базу данных, если она не существует
        await _libraryDbContext.Database.EnsureCreatedAsync(stoppingToken);

        // выводим приветственное сообщение
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Привет мир!");

        // останавливаем приложение
        _appLifetime.StopApplication();
    }
}
