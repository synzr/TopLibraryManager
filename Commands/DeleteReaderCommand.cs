using TopLibraryManager.Services;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Commands;

public class DeleteReaderCommand : BaseCommand
{
    public DeleteReaderCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService,
        IBookService bookService,
        IReaderService readerService) 
        : base(consoleUIService, librarianService, bookService, readerService)
    {
    }
    
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Удаление читателя ===");
        
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать ID читателя. Использование: удалитьчитателя <ID>");
            return true;
        }
        
        if (!int.TryParse(args[0], out int readerId))
        {
            _consoleUIService.WriteLine("Некорректный ID читателя. ID должен быть числом.");
            return true;
        }
        
        // Получаем информацию о читателе перед удалением
        var reader = _readerService.GetReaderById(readerId);
        if (reader == null)
        {
            _consoleUIService.WriteLine($"Читатель с ID {readerId} не найден.");
            return true;
        }
        
        _consoleUIService.WriteLine($"Найден читатель:");
        _consoleUIService.WriteLine($"ФИО: {reader.Fio}");
        _consoleUIService.WriteLine($"Email: {reader.Email}");
        _consoleUIService.WriteLine($"Телефон: {reader.Phone}");
        _consoleUIService.WriteLine($"Дата регистрации: {reader.RegisteredAt:dd.MM.yyyy HH:mm}");
        
        // Запрашиваем подтверждение
        string? confirmation = _consoleUIService.ReadLine("\nВы уверены, что хотите удалить этого читателя? (да/нет): ");
        
        if (!string.Equals(confirmation, "да", StringComparison.OrdinalIgnoreCase))
        {
            _consoleUIService.WriteLine("Удаление отменено.");
            return true;
        }
        
        try
        {
            bool deleted = _readerService.DeleteReaderById(readerId);
            
            if (deleted)
            {
                _consoleUIService.WriteLine($"\nЧитатель '{reader.Fio}' успешно удален.");
            }
            else
            {
                _consoleUIService.WriteLine($"\nОшибка: читатель с ID {readerId} не найден.");
            }
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при удалении читателя: {ex.Message}");
        }
        
        return true;
    }
}