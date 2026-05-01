using TopLibraryManager.Services;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Commands;

public class GetLibrarianCommand : BaseCommand
{
    public GetLibrarianCommand(
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
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать логин или ID библиотекаря. Использование: кто <логин/ID>");
            return true;
        }

        string input = args[0];
        _consoleUIService.WriteLine("\n=== Информация о библиотекаре ===");

        Librarian? librarian = null;

        if (int.TryParse(input, out int id))
        {
            librarian = _librarianService.GetLibrarianById(id);
        }
        else
        {
            librarian = _librarianService.GetLibrarianByLogin(input);
        }

        if (librarian == null)
        {
            _consoleUIService.WriteLine($"Библиотекарь '{input}' не найден.");
            return true;
        }

        _consoleUIService.WriteLine($"ID: {librarian.Id}");
        _consoleUIService.WriteLine($"ФИО: {librarian.Fio}");
        _consoleUIService.WriteLine($"Логин: {librarian.Login}");

        return true;
    }
}
