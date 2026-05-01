using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class UnknownCommand : BaseCommand
{
    public UnknownCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService)
        : base(consoleUIService, librarianService, bookService)
    {
    }

    /// <inheritdoc />
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLines([
            "Неизвестная команда.",
            "\tИспользуйте команду 'помощь' для просмотра списка доступных команд."
        ]);
        return true;
    }
}