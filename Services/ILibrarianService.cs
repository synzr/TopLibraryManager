using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Services;

public interface ILibrarianService
{
    /// <summary>
    /// Регистрация нового библиотекаря
    /// </summary>
    /// <param name="fio">ФИО</param>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль в открытом виде</param>
    /// <returns>Созданный библиотекарь</returns>
    public Librarian RegisterLibrarian(string fio, string login, string password);

    /// <summary>
    /// Получение библиотекаря по ID
    /// </summary>
    /// <param name="id">ID библиотекаря</param>
    /// <returns>Библиотекарь или null, если не найден</returns>
    public Librarian? GetLibrarianById(int id);

    /// <summary>
    /// Получение библиотекаря по логину
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns>Библиотекарь или null, если не найден</returns>
    public Librarian? GetLibrarianByLogin(string login);

    /// <summary>
    /// Удаление библиотекаря по ID
    /// </summary>
    /// <param name="id">ID библиотекаря</param>
    /// <returns>True, если удален, false, если не найден</returns>
    public bool DeleteLibrarianById(int id);

    /// <summary>
    /// Удаление библиотекаря по логину
    /// </summary>
    /// <param name="login">Логин</param>
    /// <returns>True, если удален, false, если не найден</returns>
    public bool DeleteLibrarianByLogin(string login);

    /// <summary>
    /// Проверка существования хотя бы одного библиотекаря
    /// </summary>
    /// <returns>True, если существует хотя бы один библиотекарь</returns>
    public bool AnyLibrarianExists();

    /// <summary>
    /// Аутентификация библиотекаря по логину и паролю
    /// </summary>
    /// <param name="login">Логин</param>
    /// <param name="password">Пароль в открытом виде</param>
    /// <returns>Аутентифицированный библиотекарь или null, если аутентификация не удалась</returns>
    public Librarian? Authenticate(string login, string password);
}