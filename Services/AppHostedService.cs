using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Text;
using TopLibraryManager.Data;
using TopLibraryManager.Models.Entities;
using TopLibraryManager.Utils;
using static System.Formats.Asn1.AsnWriter;

namespace TopLibraryManager.Services;

public class AppHostedService(
    LibraryDbContext libraryDbContext,
    IConsoleUIService consoleUIService,
    ICommandProcessorService commandProcessorService,
    IHostApplicationLifetime appLifetime,
    IAuthService authService
) : BackgroundService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;

    /// <summary>
    /// Сервис консольного пользовательского интерфейса
    /// </summary>
    private readonly IConsoleUIService _consoleUIService = consoleUIService;

    /// <summary>
    /// Сервис обработки пользовательские команд
    /// </summary>
    private readonly ICommandProcessorService _commandProcessorService = commandProcessorService;

    /// <summary>
    /// Менеджер жизненного цикла приложения
    /// </summary>
    private readonly IHostApplicationLifetime _appLifetime = appLifetime;

    /// <summary>
    /// Сервис аутентификации
    /// </summary>
    private readonly IAuthService _authService = authService;

    /// <summary>
    /// Текущий аутентифицированный библиотекарь
    /// </summary>
    private Librarian? _currentLibrarian;

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // создаем базу данных, если она не существует
        await _libraryDbContext.Database.EnsureCreatedAsync(cancellationToken);

        // аутентификация библиотекаря
        _currentLibrarian = _authService.Authenticate();
        _consoleUIService.WriteLine($"\nДобро пожаловать, {_currentLibrarian.Fio}!");

        // обрабатываем пользовательские команды
        ProcessCommandLine(cancellationToken);

        // останавливаем приложение
        _appLifetime.StopApplication();
    }

    private void ProcessCommandLine(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var commandRequest = _consoleUIService.ReadCommandRequest();

            if (commandRequest != null)
            {
                // обработка пользовательских команд
                var needToContinue = _commandProcessorService.ProcessCommand(commandRequest);

                if (!needToContinue)
                {
                    break;
                }
            }
        }
    }
}
