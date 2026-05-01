using TopLibraryManager.Models.Entities;
using TopLibraryManager.Models.Enums;

namespace TopLibraryManager.Services.Interfaces;

public interface ILoanService
{
    /// <summary>
    /// Создание новой выдачи книги
    /// </summary>
    /// <param name="bookId">ID книги</param>
    /// <param name="readerId">ID читателя</param>
    /// <param name="librarianId">ID библиотекаря</param>
    /// <param name="returnAt">Планируемая дата возврата</param>
    /// <returns>Созданная выдача</returns>
    Loan CreateLoan(int bookId, int readerId, int librarianId, DateOnly returnAt);
    
    /// <summary>
    /// Возврат книги по ID выдачи
    /// </summary>
    /// <param name="loanId">ID выдачи</param>
    /// <returns>Обновленная выдача или null, если не найдена</returns>
    Loan? ReturnBook(int loanId);
    
    /// <summary>
    /// Получение выдачи по ID
    /// </summary>
    /// <param name="id">ID выдачи</param>
    /// <returns>Выдача или null, если не найдена</returns>
    Loan? GetLoanById(int id);
    
    /// <summary>
    /// Получение активных выдач (книги еще не возвращены)
    /// </summary>
    /// <returns>Коллекция активных выдач</returns>
    IEnumerable<Loan> GetActiveLoans();
    
    /// <summary>
    /// Получение выдач по читателю
    /// </summary>
    /// <param name="readerId">ID читателя</param>
    /// <returns>Коллекция выдач читателя</returns>
    IEnumerable<Loan> GetLoansByReader(int readerId);
    
    /// <summary>
    /// Получение выдач по книге
    /// </summary>
    /// <param name="bookId">ID книги</param>
    /// <returns>Коллекция выдач книги</returns>
    IEnumerable<Loan> GetLoansByBook(int bookId);
    
    /// <summary>
    /// Поиск выдач с фильтрами
    /// </summary>
    /// <param name="bookId">Фильтр по ID книги (null, если не фильтровать)</param>
    /// <param name="readerId">Фильтр по ID читателя (null, если не фильтровать)</param>
    /// <param name="status">Фильтр по статусу (null, если не фильтровать)</param>
    /// <param name="issuedAfter">Фильтр по дате выдачи после указанной (null, если не фильтровать)</param>
    /// <returns>Коллекция найденных выдач</returns>
    IEnumerable<Loan> SearchLoans(int? bookId = null, int? readerId = null, 
                                  LoanStatus? status = null, DateOnly? issuedAfter = null);
    
    /// <summary>
    /// Расчет штрафа для выдачи (если есть просрочка)
    /// </summary>
    /// <param name="loanId">ID выдачи</param>
    /// <returns>Сумма штрафа в копейках/центах</returns>
    int CalculateFineAmount(int loanId);
}