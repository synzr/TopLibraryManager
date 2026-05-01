using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class HelloCommand : BaseCommand
{
    public HelloCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService,
        IReaderService readerService)
        : base(consoleUIService, librarianService, bookService, readerService)
    {
    }

    /// <inheritdoc />
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("Привет, мир!");
        return true;
    }
}