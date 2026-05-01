using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class ExitCommand : BaseCommand
{
    public ExitCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService)
        : base(consoleUIService, librarianService, bookService)
    {
    }

    /// <inheritdoc />
    public override bool Execute(string[] args)
    {
        // Вывод не требуется, просто возвращаем false для сигнала выхода
        return false;
    }
}