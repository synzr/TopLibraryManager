using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Loans;

public class ListActiveLoansCommand : ICommand
{
    public string Name => "активныевыдачи";
    public IEnumerable<string> Aliases => new[] { "выдачи", "activeloans" };
    public string Description => "Просмотр всех активных выдач (книги не возвращены)";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly ILoanService _loanService;

    public ListActiveLoansCommand(
        IConsoleUIService consoleUIService,
        ILoanService loanService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _loanService = loanService ?? throw new ArgumentNullException(nameof(loanService));
    }
    
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Активные выдачи книг ===");
        
        var activeLoans = _loanService.GetActiveLoans().ToList();
        
        if (!activeLoans.Any())
        {
            _consoleUIService.WriteLine("Нет активных выдач.");
            return true;
        }
        
        _consoleUIService.WriteLine($"Найдено активных выдач: {activeLoans.Count}\n");
        
        foreach (var loan in activeLoans)
        {
            _consoleUIService.WriteLine($"ID выдачи: {loan.Id}");
            _consoleUIService.WriteLine($"  Книга: {loan.Book.Title} ({loan.Book.Author})");
            _consoleUIService.WriteLine($"  Читатель: {loan.Reader.Fio}");
            _consoleUIService.WriteLine($"  Дата выдачи: {loan.IssuedAt:dd.MM.yyyy}");
            _consoleUIService.WriteLine($"  Дата возврата: {loan.ReturnAt:dd.MM.yyyy}");
            
            if (loan.DaysOverdue > 0)
            {
                _consoleUIService.WriteLine($"  ПРОСРОЧКА: {loan.DaysOverdue} дней");
                _consoleUIService.WriteLine($"  Штраф: {_loanService.CalculateFineAmount(loan.Id) / 100.0:F2} руб.");
            }
            else
            {
                var daysLeft = (loan.ReturnAt.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
                _consoleUIService.WriteLine($"  Осталось дней: {daysLeft}");
            }
            
            _consoleUIService.WriteLine($"  Статус: {GetLoanStatusDescription(loan.Status)}");
            _consoleUIService.WriteLine(string.Empty);
        }
        
        return true;
    }
    
    private string GetLoanStatusDescription(Models.Enums.LoanStatus status)
    {
        return status switch
        {
            Models.Enums.LoanStatus.Active => "Активна",
            Models.Enums.LoanStatus.Returned => "Возвращена вовремя",
            Models.Enums.LoanStatus.Overdue => "Просрочена",
            Models.Enums.LoanStatus.Completed => "Завершена (с просрочкой)",
            _ => "Неизвестно"
        };
    }
}