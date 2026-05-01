using System.Collections.Generic;
using TopLibraryManager.Commands;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Librarians;

public class RegisterLibrarianCommand : ICommand
{
    public string Name => "новыйбиблиотекарь";
    public IEnumerable<string> Aliases => new[] { "регистрациябиблиотекаря", "registerlibrarian" };
    public string Description => "Регистрация нового библиотекаря";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly ILibrarianService _librarianService;

    public RegisterLibrarianCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService)
    {
        _consoleUIService = consoleUIService;
        _librarianService = librarianService;
    }

    /// <inheritdoc />
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Регистрация библиотекаря ===");

        // запрашиваем ФИО, логин и пароль
        string? fio, login, password;
        do
        {
            fio = _consoleUIService.ReadLine("Введите ФИО: ");
        } while (fio == null);
        do
        {
            login = _consoleUIService.ReadLine("Введите логин: ");
        } while (login == null);
        do
        {
            password = _consoleUIService.ReadPassword("Пароль: ");
        } while (password == null);

        try
        {
            var librarian = _librarianService.RegisterLibrarian(fio, login, password);
            _consoleUIService.WriteLine($"\nБиблиотекарь '{librarian.Fio}' успешно зарегистрирован с логином '{librarian.Login}' (ID: {librarian.Id}).");
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при регистрации: {ex.Message}");
        }

        return true; // Продолжить после регистрации
    }
}