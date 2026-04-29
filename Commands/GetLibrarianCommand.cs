using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class GetLibrarianCommand : BaseCommand
{
    public GetLibrarianCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService) 
        : base(consoleUIService, librarianService)
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

        Models.Entities.Librarian? librarian = null;

        // пытаемся интерпретировать ввод как ID
        if (int.TryParse(input, out int id))
        {
            librarian = _librarianService.GetLibrarianById(id);
        }
        else
        {
            // иначе ищем по логину
            librarian = _librarianService.GetLibrarianByLogin(input);
        }

        if (librarian == null)
        {
            _consoleUIService.WriteLine($"\nБиблиотекарь не найден.");
            return true;
        }

        _consoleUIService.WriteLines([
            $"\nИнформация о библиотекаре:",
            $"  ID: {librarian.Id}",
            $"  ФИО: {librarian.Fio}",
            $"  Логин: {librarian.Login}"
        ]);

        return true;
    }
}