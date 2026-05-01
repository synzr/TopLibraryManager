using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class DeleteLibrarianCommand : BaseCommand
{
    public DeleteLibrarianCommand(
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
            _consoleUIService.WriteLine("Необходимо указать логин или ID библиотекаря. Использование: удлбиб <логин/ID>");
            return true;
        }

        string input = args[0];
        _consoleUIService.WriteLine("\n=== Удаление библиотекаря ===");

        bool success = false;

        // пытаемся интерпретировать ввод как ID
        if (int.TryParse(input, out int id))
        {
            success = _librarianService.DeleteLibrarianById(id);
        }
        else
        {
            // иначе удаляем по логину
            success = _librarianService.DeleteLibrarianByLogin(input);
        }

        if (success)
        {
            _consoleUIService.WriteLine($"\nБиблиотекарь успешно удален.");
        }
        else
        {
            _consoleUIService.WriteLine($"\nБиблиотекарь не найден.");
        }

        return true;
    }
}
