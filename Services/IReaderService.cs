using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Services;

public interface IReaderService
{
    /// <summary>
    /// Создание нового читателя
    /// </summary>
    /// <param name="fio">ФИО читателя</param>
    /// <param name="email">Email читателя</param>
    /// <param name="phone">Телефон читателя</param>
    /// <returns>Созданный читатель</returns>
    Reader CreateReader(string fio, string email, string phone);
    
    /// <summary>
    /// Получение читателя по ID
    /// </summary>
    /// <param name="id">ID читателя</param>
    /// <returns>Читатель или null, если не найден</returns>
    Reader? GetReaderById(int id);
    
    /// <summary>
    /// Обновление существующего читателя
    /// </summary>
    /// <param name="id">ID читателя</param>
    /// <param name="fio">Новое ФИО (null, если не изменять)</param>
    /// <param name="email">Новый email (null, если не изменять)</param>
    /// <param name="phone">Новый телефон (null, если не изменять)</param>
    /// <returns>Обновленный читатель или null, если читатель не найден</returns>
    Reader? UpdateReader(int id, string? fio = null, string? email = null, string? phone = null);
    
    /// <summary>
    /// Удаление читателя по ID
    /// </summary>
    /// <param name="id">ID читателя</param>
    /// <returns>True, если удален, false, если не найден</returns>
    bool DeleteReaderById(int id);
    
    /// <summary>
    /// Поиск читателей с опциональными фильтрами
    /// </summary>
    /// <param name="fio">Фильтр по ФИО (null, если не фильтровать)</param>
    /// <param name="email">Фильтр по email (null, если не фильтровать)</param>
    /// <param name="phone">Фильтр по телефону (null, если не фильтровать)</param>
    /// <returns>Коллекция найденных читателей</returns>
    IEnumerable<Reader> SearchReaders(string? fio = null, string? email = null, string? phone = null);
    
    /// <summary>
    /// Проверка существования хотя бы одного читателя
    /// </summary>
    /// <returns>True, если существует хотя бы один читатель</returns>
    bool AnyReadersExist();
}