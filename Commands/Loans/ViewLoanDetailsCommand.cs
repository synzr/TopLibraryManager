using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Loans;

public class ViewLoanDetailsCommand : ICommand
{
    public string Name => "посмотретьвыдачу";
    public IEnumerable<string> Aliases => new[] { "деталивыдачи", "viewloan" };
    public string Description => "Просмотр детальной информации о выдаче и связанных штрафах";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly ILoanService _loanService;
    private readonly IFineService _fineService;

    public ViewLoanDetailsCommand(
        IConsoleUIService consoleUIService,
        ILoanService loanService,
        IFineService fineService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _loanService = loanService ?? throw new ArgumentNullException(nameof(loanService));
        _fineService = fineService ?? throw new ArgumentNullException(nameof(fineService));
    }
    
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Просмотр деталей выдачи ===");
        
        // Получение ID выдачи
        int loanId;
        while (true)
        {
            var loanIdInput = _consoleUIService.ReadLine("ID выдачи: ");
            if (int.TryParse(loanIdInput, out loanId) && loanId > 0)
            {
                break;
            }
            _consoleUIService.WriteLine("Некорректный ID выдачи. Попробуйте снова.");
        }
        
        var loan = _loanService.GetLoanById(loanId);
        if (loan == null)
        {
            _consoleUIService.WriteLine($"Выдача с ID {loanId} не найдена.");
            return true;
        }
        
        // Отображение информации о выдаче
        _consoleUIService.WriteLine("\n=== Информация о выдаче ===");
        _consoleUIService.WriteLine($"ID выдачи: {loan.Id}");
        _consoleUIService.WriteLine($"Статус: {GetLoanStatusDescription(loan.Status)}");
        _consoleUIService.WriteLine($"Дата выдачи: {loan.IssuedAt:dd.MM.yyyy HH:mm}");
        _consoleUIService.WriteLine($"Планируемая дата возврата: {loan.ReturnAt:dd.MM.yyyy}");
        
        if (loan.ReturnedAt.HasValue)
        {
            _consoleUIService.WriteLine($"Фактическая дата возврата: {loan.ReturnedAt:dd.MM.yyyy HH:mm}");
        }
        
        _consoleUIService.WriteLine($"Дней просрочки: {loan.DaysOverdue}");
        
        // Информация о книге
        _consoleUIService.WriteLine("\n=== Информация о книге ===");
        _consoleUIService.WriteLine($"ID книги: {loan.Book.Id}");
        _consoleUIService.WriteLine($"Название: {loan.Book.Title}");
        _consoleUIService.WriteLine($"Автор: {loan.Book.Author}");
        _consoleUIService.WriteLine($"Жанр: {loan.Book.Genre}");
        _consoleUIService.WriteLine($"Год издания: {loan.Book.Year}");
        
        // Информация о читателе
        _consoleUIService.WriteLine("\n=== Информация о читателе ===");
        _consoleUIService.WriteLine($"ID читателя: {loan.Reader.Id}");
        _consoleUIService.WriteLine($"ФИО: {loan.Reader.Fio}");
        _consoleUIService.WriteLine($"Email: {loan.Reader.Email}");
        _consoleUIService.WriteLine($"Телефон: {loan.Reader.Phone}");
        _consoleUIService.WriteLine($"Дата регистрации: {loan.Reader.RegisteredAt:dd.MM.yyyy}");
        
        // Информация о библиотекаре
        _consoleUIService.WriteLine("\n=== Информация о библиотекаре ===");
        _consoleUIService.WriteLine($"ID библиотекаря: {loan.Librarian.Id}");
        _consoleUIService.WriteLine($"ФИО: {loan.Librarian.Fio}");
        _consoleUIService.WriteLine($"Логин: {loan.Librarian.Login}");
        
        // Штрафы по выдаче
        var fines = _fineService.GetFinesByLoan(loanId).ToList();
        if (fines.Any())
        {
            _consoleUIService.WriteLine("\n=== Штрафы по выдаче ===");
            foreach (var fine in fines)
            {
                _consoleUIService.WriteLine($"  ID штрафа: {fine.Id}");
                _consoleUIService.WriteLine($"  Сумма: {fine.Amount / 100.0:F2} руб.");
                _consoleUIService.WriteLine($"  Статус: {(fine.Status == Models.Enums.FineStatus.Paid ? "Оплачен" : "Не оплачен")}");
                _consoleUIService.WriteLine($"  Дата создания: {fine.CreatedAt:dd.MM.yyyy HH:mm}");
                if (fine.PaidAt.HasValue)
                {
                    _consoleUIService.WriteLine($"  Дата оплаты: {fine.PaidAt:dd.MM.yyyy HH:mm}");
                }
                _consoleUIService.WriteLine(string.Empty);
            }
        }
        else
        {
            _consoleUIService.WriteLine("\nШтрафов по этой выдаче нет.");
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