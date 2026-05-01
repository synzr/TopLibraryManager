using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class HelloCommand : BaseCommand
{
    public HelloCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService)
        : base(consoleUIService, librarianService, bookService)
    {
    }

    /// <inheritdoc />
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("Привет, мир!");
        return true;
    }
}