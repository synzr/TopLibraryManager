using Microsoft.EntityFrameworkCore;
using TopLibraryManager.Data;
using TopLibraryManager.Models.Entities;
using TopLibraryManager.Models.Enums;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Services.Implementations;

public class FineService(LibraryDbContext libraryDbContext) : IFineService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;
    
    /// <inheritdoc />
    public Fine CreateFineForLoan(int loanId, int pricePerDay)
    {
        var loan = _libraryDbContext.Loans
            .Include(l => l.Reader)
            .FirstOrDefault(l => l.Id == loanId);
            
        if (loan == null)
            throw new ArgumentException($"Выдача с ID {loanId} не найдена", nameof(loanId));
        
        // Проверяем, не создан ли уже штраф для этой выдачи
        var existingFine = _libraryDbContext.Fines
            .FirstOrDefault(f => f.LoanId == loanId && f.Status == FineStatus.Unpaid);
        if (existingFine != null)
            throw new InvalidOperationException($"Для выдачи ID {loanId} уже существует неоплаченный штраф (ID: {existingFine.Id})");
        
        // Рассчитываем сумму штрафа
        var daysOverdue = loan.DaysOverdue;
        if (daysOverdue <= 0)
            throw new InvalidOperationException($"Для выдачи ID {loanId} нет просрочки (дней просрочки: {daysOverdue})");
        
        var amount = daysOverdue * pricePerDay;
        
        var fine = new Fine
        {
            LoanId = loanId,
            PricePerDay = pricePerDay,
            Amount = amount,
            Status = FineStatus.Unpaid,
            CreatedAt = DateTime.Now,
            PaidAt = null
        };
        
        _libraryDbContext.Fines.Add(fine);
        _libraryDbContext.SaveChanges();
        return fine;
    }
    
    /// <inheritdoc />
    public Fine? GetFineById(int id)
    {
        return _libraryDbContext.Fines
            .Include(f => f.Loan)
            .ThenInclude(l => l.Book)
            .Include(f => f.Loan)
            .ThenInclude(l => l.Reader)
            .FirstOrDefault(f => f.Id == id);
    }
    
    /// <inheritdoc />
    public IEnumerable<Fine> GetFinesByLoan(int loanId)
    {
        return _libraryDbContext.Fines
            .Include(f => f.Loan)
            .ThenInclude(l => l.Book)
            .Where(f => f.LoanId == loanId)
            .OrderByDescending(f => f.CreatedAt)
            .ToList();
    }
    
    /// <inheritdoc />
    public IEnumerable<Fine> GetUnpaidFinesByReader(int readerId)
    {
        return _libraryDbContext.Fines
            .Include(f => f.Loan)
            .ThenInclude(l => l.Book)
            .Where(f => f.Loan.ReaderId == readerId && f.Status == FineStatus.Unpaid)
            .OrderByDescending(f => f.CreatedAt)
            .ToList();
    }
    
    /// <inheritdoc />
    public bool MarkFineAsPaid(int fineId)
    {
        var fine = GetFineById(fineId);
        if (fine == null)
            return false;
            
        if (fine.Status == FineStatus.Paid)
            return true; // Уже оплачен
            
        fine.Status = FineStatus.Paid;
        fine.PaidAt = DateTime.Now;
        
        _libraryDbContext.SaveChanges();
        return true;
    }
    
    /// <inheritdoc />
    public int CalculateTotalUnpaidFinesByReader(int readerId)
    {
        return _libraryDbContext.Fines
            .Include(f => f.Loan)
            .Where(f => f.Loan.ReaderId == readerId && f.Status == FineStatus.Unpaid)
            .Sum(f => f.Amount);
    }
}