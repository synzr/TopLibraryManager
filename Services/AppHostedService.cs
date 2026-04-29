using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text;
using TopLibraryManager.Data;
using static System.Formats.Asn1.AsnWriter;

namespace TopLibraryManager.Services;

public class AppHostedService(
    LibraryDbContext libraryDbContext,
    IConsoleUIService consoleUIService,
    ICommandProcessorService commandProcessorService,
    IHostApplicationLifetime appLifetime
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

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        // создаем базу данных, если она не существует
        await _libraryDbContext.Database.EnsureCreatedAsync(cancellationToken);

        // проверяем, что пользователь аутентифицирован
        EnsureAuthenticated();

        // обрабатываем пользовательские команды
        ProcessCommandLine(cancellationToken);

        // останавливаем приложение
        _appLifetime.StopApplication();
    }

    private void EnsureAuthenticated()
    {
        // TODO
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
