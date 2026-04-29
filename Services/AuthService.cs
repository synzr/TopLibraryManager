using System;
using TopLibraryManager.Models.Entities;
using TopLibraryManager.Utils;

namespace TopLibraryManager.Services;

public class AuthService(
    ILibrarianService librarianService,
    IConsoleUIService consoleUIService
) : IAuthService
{
    /// <summary>
    /// Сервис работы с библиотекарями
    /// </summary>
    private readonly ILibrarianService _librarianService = librarianService;

    /// <summary>
    /// Сервис консольного пользовательского интерфейса
    /// </summary>
    private readonly IConsoleUIService _consoleUIService = consoleUIService;
    
    /// <inheritdoc />
    public Librarian Authenticate()
    {
        // проверяем, есть ли хотя бы один библиотекарь в базе данных
        var anyLibrarian = _librarianService.AnyLibrarianExists();
        
        if (!anyLibrarian)
        {
            _consoleUIService.WriteLines([
                "Добро пожаловать в систему управления библиотекой!",
                "Похоже, вы первый пользователь. Давайте создадим учетную запись библиотекаря."
            ]);
            
            // регистрация первого библиотекаря
            return RegisterLibrarian();
        }
        else
        {
            // вход существующего пользователя
            return Login();
        }
    }
    
    private Librarian RegisterLibrarian()
    {
        _consoleUIService.WriteLine("\n=== Регистрация библиотекаря ===");

        // запрашиваем ФИО, логин и пароль для нового библиотекаря
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
        
        // регистрируем библиотекаря через сервис
        var librarian = _librarianService.RegisterLibrarian(fio, login, password);
        
        _consoleUIService.WriteLine("\nУчетная запись успешно создана!");

        // возвращаем созданного библиотекаря
        return librarian;
    }
    
    private Librarian Login()
    {
        _consoleUIService.WriteLine("\n=== Вход в систему ===");
        
        while (true)
        {
            // запрашиваем логин и пароль
            string? login, password;
            do
            {
                login = _consoleUIService.ReadLine("Логин: ");
            } while (login == null);
            do
            {
                password = _consoleUIService.ReadPassword("Пароль: ");
            } while (password == null);
            
            // аутентифицируем библиотекаря через сервис
            var librarian = _librarianService.Authenticate(login, password);
            if (librarian == null)
            {
                _consoleUIService.WriteLine("\nНеверный логин или пароль. Попробуйте снова.\n");
                continue;
            }
            return librarian;
        }
    }
}