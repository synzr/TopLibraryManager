using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Librarians;

public class DeleteLibrarianCommand : ICommand
{
    public string Name => "удалитьбиблиотекаря";
    public IEnumerable<string> Aliases => new[] { "deletelibrarian" };
    public string Description => "Удаление библиотекаря по логину или ID";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly ILibrarianService _librarianService;

    public DeleteLibrarianCommand(
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