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

    /// <summary>
    /// Сервис работы с книгами
    /// </summary>
    protected readonly IBookService _bookService;

    protected BaseCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService)
    {
        _consoleUIService = consoleUIService;
        _librarianService = librarianService;
        _bookService = bookService;
    }

    /// <inheritdoc />
    public abstract bool Execute(string[] args);
}