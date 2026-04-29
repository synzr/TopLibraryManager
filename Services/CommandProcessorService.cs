using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TopLibraryManager.Models;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Services;

public class CommandProcessorService(
    IConsoleUIService consoleUIService,
    ILibrarianService librarianService
) : ICommandProcessorService
{
    /// <summary>
    /// Сервис консольного пользовательского интерфейса
    /// </summary>
    private readonly IConsoleUIService _consoleUIService = consoleUIService;

    /// <summary>
    /// Сервис работы с библиотекарями
    /// </summary>
    private readonly ILibrarianService _librarianService = librarianService;

    /// <inheritdoc />
    /// <exception cref="NotImplementedException"></exception>
    public bool ProcessCommand(CommandRequest commandRequest)
    {
        var command = commandRequest.Command.ToLower();

        switch (command)
        {
            // Команда приветствия
            case "привет":
                _consoleUIService.WriteLines(["Привет, мир!"]);
                break;

            // Команда регистрации библиотекаря
            case "рег":
            case "регистрациябиблиотекаря":
                HandleRegisterLibrarian();
                break;

            // Команда получения информации о библиотекаре
            case "кто":
            case "узнатьбиблиотекаря":
                HandleGetLibrarian(commandRequest.Args);
                break;

            // Команда удаления библиотекаря
            case "удлбиб":
            case "удалитьбиблиотекаря":
                HandleDeleteLibrarian(commandRequest.Args);
                break;

            // Команда выхода из приложения
            case "выход":
                return false;

            // Обработка неизвестной команды
            default:
                _consoleUIService.WriteLines([
                    "Неизвестная команда.",
                    "\tДоступные команды:",
                    "\t- 'привет' - приветствие",
                    "\t- 'рег' или 'регистрациябиблиотекаря' - регистрация библиотекаря",
                    "\t- 'кто <логин/ID>' или 'узнатьбиблиотекаря <логин/ID>' - информация о библиотекаре",
                    "\t- 'удлбиб <логин/ID>' или 'удалитьбиблиотекаря <логин/ID>' - удаление библиотекаря",
                    "\t- 'выход' - выход из приложения"
                ]);
                break;
        }

        return true;
    }

    /// <summary>
    /// Обработка регистрации библиотекаря
    /// </summary>
    private void HandleRegisterLibrarian()
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
    }

    /// <summary>
    /// Обработка получения информации о библиотекаре
    /// </summary>
    private void HandleGetLibrarian(string[] args)
    {
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать логин или ID библиотекаря. Использование: кто <логин/ID>");
            return;
        }

        string input = args[0];
        _consoleUIService.WriteLine("\n=== Информация о библиотекаре ===");

        Librarian? librarian = null;

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
            return;
        }

        _consoleUIService.WriteLines([
            $"\nИнформация о библиотекаре:",
            $"  ID: {librarian.Id}",
            $"  ФИО: {librarian.Fio}",
            $"  Логин: {librarian.Login}"
        ]);
    }

    /// <summary>
    /// Обработка удаления библиотекаря
    /// </summary>
    private void HandleDeleteLibrarian(string[] args)
    {
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать логин или ID библиотекаря. Использование: удлбиб <логин/ID>");
            return;
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
    }
}
