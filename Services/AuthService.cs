using System;
using TopLibraryManager.Data;
using TopLibraryManager.Models.Entities;
using TopLibraryManager.Utils;

namespace TopLibraryManager.Services;

public class AuthService(
    LibraryDbContext libraryDbContext,
    IConsoleUIService consoleUIService
) : IAuthService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;

    /// <summary>
    /// Сервис консольного пользовательского интерфейса
    /// </summary>
    private readonly IConsoleUIService _consoleUIService = consoleUIService;
    
    /// <inheritdoc />
    public Librarian Authenticate()
    {
        // проверяем, есть ли хотя бы один библиотекарь в базе данных
        var anyLibrarian = _libraryDbContext.Librarians.Any();
        
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
        
        // хэшируем пароль
        var hashedPassword = PasswordHasher.HashPassword(password);
        
        // создаем библиотекаря
        var librarian = new Librarian
        {
            Fio = fio,
            Login = login,
            Password = hashedPassword
        };

        // сохраняем библиотекаря в базе данных
        _libraryDbContext.Librarians.Add(librarian);
        _libraryDbContext.SaveChanges();
        
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
            
            // ищем библиотекаря по логину
            var librarian = _libraryDbContext.Librarians
                .FirstOrDefault(l => l.Login == login);
            if (librarian == null || !PasswordHasher.VerifyPassword(password, librarian.Password))
            {
                _consoleUIService.WriteLine("\nНеверный логин или пароль. Попробуйте снова.\n");
                continue;
            }
            return librarian;
        }
    }
}