using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Loans;

public class PayFineCommand : ICommand
{
    public string Name => "оплатитьштраф";
    public IEnumerable<string> Aliases => new[] { "штраф", "payfine" };
    public string Description => "Отметка штрафа как оплаченного";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly IFineService _fineService;

    public PayFineCommand(
        IConsoleUIService consoleUIService,
        IFineService fineService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _fineService = fineService ?? throw new ArgumentNullException(nameof(fineService));
    }
    
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Оплата штрафа ===");
        
        // Получение ID штрафа
        int fineId;
        while (true)
        {
            var fineIdInput = _consoleUIService.ReadLine("ID штрафа: ");
            if (int.TryParse(fineIdInput, out fineId) && fineId > 0)
            {
                break;
            }
            _consoleUIService.WriteLine("Некорректный ID штрафа. Попробуйте снова.");
        }
        
        var fine = _fineService.GetFineById(fineId);
        if (fine == null)
        {
            _consoleUIService.WriteLine($"Штраф с ID {fineId} не найден.");
            return true;
        }
        
        // Отображение информации о штрафе
        _consoleUIService.WriteLine("\n=== Информация о штрафе ===");
        _consoleUIService.WriteLine($"ID штрафа: {fine.Id}");
        _consoleUIService.WriteLine($"Сумма: {fine.Amount / 100.0:F2} руб.");
        _consoleUIService.WriteLine($"Статус: {(fine.Status == Models.Enums.FineStatus.Paid ? "Оплачен" : "Не оплачен")}");
        _consoleUIService.WriteLine($"Дата создания: {fine.CreatedAt:dd.MM.yyyy HH:mm}");
        
        if (fine.Status == Models.Enums.FineStatus.Paid)
        {
            _consoleUIService.WriteLine($"\nШтраф уже оплачен {fine.PaidAt:dd.MM.yyyy HH:mm}.");
            return true;
        }
        
        // Информация о выдаче
        _consoleUIService.WriteLine("\n=== Информация о выдаче ===");
        _consoleUIService.WriteLine($"ID выдачи: {fine.Loan.Id}");
        _consoleUIService.WriteLine($"Книга: {fine.Loan.Book.Title} ({fine.Loan.Book.Author})");
        _consoleUIService.WriteLine($"Читатель: {fine.Loan.Reader.Fio}");
        _consoleUIService.WriteLine($"Дней просрочки: {fine.Loan.DaysOverdue}");
        _consoleUIService.WriteLine($"Стоимость за день: {fine.PricePerDay / 100.0:F2} руб.");
        
        // Подтверждение оплаты
        var confirmInput = _consoleUIService.ReadLine("\nОтметить штраф как оплаченный? (да/нет): ");
        if (confirmInput?.Trim().ToLower() != "да")
        {
            _consoleUIService.WriteLine("Операция отменена.");
            return true;
        }
        
        try
        {
            var success = _fineService.MarkFineAsPaid(fineId);
            if (success)
            {
                _consoleUIService.WriteLine($"\nШтраф ID {fineId} успешно отмечен как оплаченный.");
                
                // Обновленная информация
                fine = _fineService.GetFineById(fineId);
                if (fine != null && fine.PaidAt.HasValue)
                {
                    _consoleUIService.WriteLine($"Дата оплаты: {fine.PaidAt:dd.MM.yyyy HH:mm}");
                }
            }
            else
            {
                _consoleUIService.WriteLine("\nОшибка: Не удалось отметить штраф как оплаченный.");
            }
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при оплате штрафа: {ex.Message}");
        }
        
        return true;
    }
}