using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Services;

public interface IAuthService
{
    /// <summary>
    /// Выполняет аутентификацию пользователя
    /// </summary>
    /// <returns>Аутентифицированный библиотекарь</returns>
    public Librarian Authenticate();
}