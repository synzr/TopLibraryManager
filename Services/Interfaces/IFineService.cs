using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Services.Interfaces;

public interface IFineService
{
    /// <summary>
    /// Создание штрафа для выдачи (при возврате просроченной книги)
    /// </summary>
    /// <param name="loanId">ID выдачи</param>
    /// <param name="pricePerDay">Стоимость штрафа за день в копейках/центах</param>
    /// <returns>Созданный штраф</returns>
    Fine CreateFineForLoan(int loanId, int pricePerDay);
    
    /// <summary>
    /// Получение штрафа по ID
    /// </summary>
    /// <param name="id">ID штрафа</param>
    /// <returns>Штраф или null, если не найден</returns>
    Fine? GetFineById(int id);
    
    /// <summary>
    /// Получение штрафов по выдачи
    /// </summary>
    /// <param name="loanId">ID выдачи</param>
    /// <returns>Коллекция штрафов по выдаче</returns>
    IEnumerable<Fine> GetFinesByLoan(int loanId);
    
    /// <summary>
    /// Получение неоплаченных штрафов по читателю
    /// </summary>
    /// <param name="readerId">ID читателя</param>
    /// <returns>Коллекция неоплаченных штрафов читателя</returns>
    IEnumerable<Fine> GetUnpaidFinesByReader(int readerId);
    
    /// <summary>
    /// Отметка штрафа как оплаченного
    /// </summary>
    /// <param name="fineId">ID штрафа</param>
    /// <returns>True, если штраф отмечен как оплаченный, false, если не найден</returns>
    bool MarkFineAsPaid(int fineId);
    
    /// <summary>
    /// Расчет общей суммы неоплаченных штрафов по читателю
    /// </summary>
    /// <param name="readerId">ID читателя</param>
    /// <returns>Общая сумма неоплаченных штрафов в копейках/центах</returns>
    int CalculateTotalUnpaidFinesByReader(int readerId);
}