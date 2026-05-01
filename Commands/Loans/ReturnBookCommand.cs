using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Loans;

public class ReturnBookCommand : ICommand
{
    public string Name => "вернутькнигу";
    public IEnumerable<string> Aliases => new[] { "возврат", "returnbook" };
    public string Description => "Возврат книги по ID выдачи с расчетом штрафа при просрочке";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly ILoanService _loanService;
    private readonly IFineService _fineService;

    public ReturnBookCommand(
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
        _consoleUIService.WriteLine("\n=== Возврат книги ===");
        
        // Получение ID выдачи
        int loanId;
        while (true)
        {
            var loanIdInput = _consoleUIService.ReadLine("ID выдачи: ");
            if (int.TryParse(loanIdInput, out loanId) && loanId > 0)
            {
                var loan = _loanService.GetLoanById(loanId);
                if (loan != null)
                {
                    _consoleUIService.WriteLine($"Выдача ID: {loan.Id}");
                    _consoleUIService.WriteLine($"Книга: {loan.Book.Title} ({loan.Book.Author})");
                    _consoleUIService.WriteLine($"Читатель: {loan.Reader.Fio}");
                    _consoleUIService.WriteLine($"Дата выдачи: {loan.IssuedAt:dd.MM.yyyy}");
                    _consoleUIService.WriteLine($"Планируемая дата возврата: {loan.ReturnAt:dd.MM.yyyy}");
                    
                    if (loan.ReturnedAt.HasValue)
                    {
                        _consoleUIService.WriteLine("Книга уже возвращена!");
                        return true;
                    }
                    
                    break;
                }
                _consoleUIService.WriteLine("Выдача с таким ID не найдена. Попробуйте снова.");
            }
            else
            {
                _consoleUIService.WriteLine("Некорректный ID выдачи. Попробуйте снова.");
            }
        }
        
        try
        {
            var returnedLoan = _loanService.ReturnBook(loanId);
            if (returnedLoan == null)
            {
                _consoleUIService.WriteLine("\nОшибка: Выдача не найдена.");
                return true;
            }
            
            _consoleUIService.WriteLine($"\nКнига успешно возвращена.");
            _consoleUIService.WriteLine($"Фактическая дата возврата: {returnedLoan.ReturnedAt:dd.MM.yyyy HH:mm}");
            _consoleUIService.WriteLine($"Статус: {GetLoanStatusDescription(returnedLoan.Status)}");
            
            // Проверка на просрочку
            if (returnedLoan.DaysOverdue > 0)
            {
                _consoleUIService.WriteLine($"\nВНИМАНИЕ: Просрочка {returnedLoan.DaysOverdue} дней!");
                _consoleUIService.WriteLine($"Сумма штрафа: {_loanService.CalculateFineAmount(loanId) / 100.0:F2} руб.");
                
                var createFineInput = _consoleUIService.ReadLine("Создать штраф? (да/нет): ");
                if (createFineInput?.Trim().ToLower() == "да")
                {
                    try
                    {
                        var fine = _fineService.CreateFineForLoan(loanId, 50); // 50 копеек в день
                        _consoleUIService.WriteLine($"Штраф создан (ID: {fine.Id}). Сумма: {fine.Amount / 100.0:F2} руб.");
                    }
                    catch (Exception ex)
                    {
                        _consoleUIService.WriteLine($"Ошибка при создании штрафа: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при возврате книги: {ex.Message}");
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