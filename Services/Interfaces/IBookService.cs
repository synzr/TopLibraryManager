using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Services.Interfaces;

public interface IBookService
{
    /// <summary>
    /// Создание новой книги
    /// </summary>
    /// <param name="title">Название книги</param>
    /// <param name="author">Автор</param>
    /// <param name="genre">Жанр</param>
    /// <param name="year">Год издания</param>
    /// <returns>Созданная книга</returns>
    Book CreateBook(string title, string author, string genre, short year);
    
    /// <summary>
    /// Получение книги по ID
    /// </summary>
    /// <param name="id">ID книги</param>
    /// <returns>Книга или null, если не найдена</returns>
    Book? GetBookById(int id);
    
    /// <summary>
    /// Обновление существующей книги
    /// </summary>
    /// <param name="id">ID книги</param>
    /// <param name="title">Новое название (null, если не изменять)</param>
    /// <param name="author">Новый автор (null, если не изменять)</param>
    /// <param name="genre">Новый жанр (null, если не изменять)</param>
    /// <param name="year">Новый год издания (null, если не изменять)</param>
    /// <returns>Обновленная книга или null, если книга не найдена</returns>
    Book? UpdateBook(int id, string? title = null, string? author = null, string? genre = null, short? year = null);
    
    /// <summary>
    /// Удаление книги по ID
    /// </summary>
    /// <param name="id">ID книги</param>
    /// <returns>True, если удалена, false, если не найдена</returns>
    bool DeleteBookById(int id);
    
    /// <summary>
    /// Поиск книг с опциональными фильтрами
    /// </summary>
    /// <param name="title">Фильтр по названию (null, если не фильтровать)</param>
    /// <param name="author">Фильтр по автору (null, если не фильтровать)</param>
    /// <param name="genre">Фильтр по жанру (null, если не фильтровать)</param>
    /// <param name="year">Фильтр по году издания (null, если не фильтровать)</param>
    /// <returns>Коллекция найденных книг</returns>
    IEnumerable<Book> SearchBooks(string? title = null, string? author = null, string? genre = null, short? year = null);
    
    /// <summary>
    /// Проверка существования хотя бы одной книги
    /// </summary>
    /// <returns>True, если существует хотя бы одна книга</returns>
    bool AnyBooksExist();
}