using Microsoft.EntityFrameworkCore;
using TopLibraryManager.Data;
using TopLibraryManager.Models.Entities;
using TopLibraryManager.Models.Enums;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Services.Implementations;

public class LoanService(LibraryDbContext libraryDbContext) : ILoanService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;
    
    /// <inheritdoc />
    public Loan CreateLoan(int bookId, int readerId, int librarianId, DateOnly returnAt)
    {
        // Проверка существования книги, читателя и библиотекаря
        var book = _libraryDbContext.Books.Find(bookId);
        if (book == null)
            throw new ArgumentException($"Книга с ID {bookId} не найдена", nameof(bookId));
            
        var reader = _libraryDbContext.Readers.Find(readerId);
        if (reader == null)
            throw new ArgumentException($"Читатель с ID {readerId} не найден", nameof(readerId));
            
        var librarian = _libraryDbContext.Librarians.Find(librarianId);
        if (librarian == null)
            throw new ArgumentException($"Библиотекарь с ID {librarianId} не найден", nameof(librarianId));
        
        // Проверка, что книга не выдана в данный момент
        var activeLoanForBook = _libraryDbContext.Loans
            .FirstOrDefault(l => l.BookId == bookId && l.Status == LoanStatus.Active);
        if (activeLoanForBook != null)
            throw new InvalidOperationException($"Книга с ID {bookId} уже выдана (ID выдачи: {activeLoanForBook.Id})");
        
        var loan = new Loan
        {
            BookId = bookId,
            ReaderId = readerId,
            LibrarianId = librarianId,
            IssuedAt = DateTime.Now,
            ReturnAt = returnAt,
            ReturnedAt = null,
            Status = LoanStatus.Active
        };
        
        _libraryDbContext.Loans.Add(loan);
        _libraryDbContext.SaveChanges();
        return loan;
    }
    
    /// <inheritdoc />
    public Loan? ReturnBook(int loanId)
    {
        var loan = GetLoanById(loanId);
        if (loan == null)
            return null;
            
        if (loan.Status == LoanStatus.Returned || loan.Status == LoanStatus.Completed)
            throw new InvalidOperationException($"Книга по выдаче ID {loanId} уже возвращена");
        
        loan.ReturnedAt = DateTime.Now;
        
        // Определяем статус в зависимости от просрочки
        if (loan.ReturnedAt.Value.Date <= loan.ReturnAt.ToDateTime(TimeOnly.MinValue))
        {
            loan.Status = LoanStatus.Returned;
        }
        else
        {
            loan.Status = LoanStatus.Completed;
        }
        
        _libraryDbContext.SaveChanges();
        return loan;
    }
    
    /// <inheritdoc />
    public Loan? GetLoanById(int id)
    {
        return _libraryDbContext.Loans
            .Include(l => l.Book)
            .Include(l => l.Reader)
            .Include(l => l.Librarian)
            .FirstOrDefault(l => l.Id == id);
    }
    
    /// <inheritdoc />
    public IEnumerable<Loan> GetActiveLoans()
    {
        return _libraryDbContext.Loans
            .Include(l => l.Book)
            .Include(l => l.Reader)
            .Include(l => l.Librarian)
            .Where(l => l.Status == LoanStatus.Active || l.Status == LoanStatus.Overdue)
            .OrderByDescending(l => l.IssuedAt)
            .ToList();
    }
    
    /// <inheritdoc />
    public IEnumerable<Loan> GetLoansByReader(int readerId)
    {
        return _libraryDbContext.Loans
            .Include(l => l.Book)
            .Include(l => l.Librarian)
            .Where(l => l.ReaderId == readerId)
            .OrderByDescending(l => l.IssuedAt)
            .ToList();
    }
    
    /// <inheritdoc />
    public IEnumerable<Loan> GetLoansByBook(int bookId)
    {
        return _libraryDbContext.Loans
            .Include(l => l.Reader)
            .Include(l => l.Librarian)
            .Where(l => l.BookId == bookId)
            .OrderByDescending(l => l.IssuedAt)
            .ToList();
    }
    
    /// <inheritdoc />
    public IEnumerable<Loan> SearchLoans(int? bookId = null, int? readerId = null, 
                                         LoanStatus? status = null, DateOnly? issuedAfter = null)
    {
        var query = _libraryDbContext.Loans
            .Include(l => l.Book)
            .Include(l => l.Reader)
            .Include(l => l.Librarian)
            .AsQueryable();
        
        if (bookId.HasValue)
            query = query.Where(l => l.BookId == bookId.Value);
        if (readerId.HasValue)
            query = query.Where(l => l.ReaderId == readerId.Value);
        if (status.HasValue)
            query = query.Where(l => l.Status == status.Value);
        if (issuedAfter.HasValue)
            query = query.Where(l => l.IssuedAt.Date >= issuedAfter.Value.ToDateTime(TimeOnly.MinValue));
            
        return query.OrderByDescending(l => l.IssuedAt).ToList();
    }
    
    /// <inheritdoc />
    public int CalculateFineAmount(int loanId)
    {
        var loan = GetLoanById(loanId);
        if (loan == null)
            return 0;
            
        // Используем DaysOverdue из сущности Loan
        return loan.DaysOverdue * 50; // Фиксированная ставка 50 копеек/центов в день
    }
}