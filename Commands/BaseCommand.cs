using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public abstract class BaseCommand : ICommand
{
    /// <summary>
    /// Сервис консольного пользовательского интерфейса
    /// </summary>
    protected readonly IConsoleUIService _consoleUIService;

    /// <summary>
    /// Сервис работы с библиотекарями
    /// </summary>
    protected readonly ILibrarianService _librarianService;

    protected BaseCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService)
    {
        _consoleUIService = consoleUIService;
        _librarianService = librarianService;
    }

    /// <inheritdoc />
    public abstract bool Execute(string[] args);
}