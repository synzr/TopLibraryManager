using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class RegisterLibrarianCommand : BaseCommand
{
    public RegisterLibrarianCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService) 
        : base(consoleUIService, librarianService)
    {
    }

    /// <inheritdoc />
    public override bool Execute(string[] args)
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