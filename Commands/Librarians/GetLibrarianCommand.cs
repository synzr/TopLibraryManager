using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Librarians;

public class GetLibrarianCommand : ICommand
{
    public string Name => "посмотретьбиблиотекаря";
    public IEnumerable<string> Aliases => new[] { "getlibrarian" };
    public string Description => "Получение информации о библиотекаре по логину или ID";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly ILibrarianService _librarianService;

    public GetLibrarianCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService)
    {
        _consoleUIService = consoleUIService;
        _librarianService = librarianService;
    }

    /// <inheritdoc />
    public bool Execute(string[] args)
    {
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать логин или ID библиотекаря. Использование: кто <логин/ID>");
            return true;
        }

        string input = args[0];
        _consoleUIService.WriteLine("\n=== Информация о библиотекаре ===");

        var librarian = (Models.Entities.Librarian?)null;

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