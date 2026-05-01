using TopLibraryManager.Services;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Commands;

public class GetReaderCommand : BaseCommand
{
    public GetReaderCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService,
        IBookService bookService,
        IReaderService readerService) 
        : base(consoleUIService, librarianService, bookService, readerService)
    {
    }
    
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Получение информации о читателе ===");
        
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать ID читателя. Использование: получитьчитателя <ID>");
            return true;
        }
        
        if (!int.TryParse(args[0], out int readerId))
        {
            _consoleUIService.WriteLine("Некорректный ID читателя. ID должен быть числом.");
            return true;
        }
        
        try
        {
            var reader = _readerService.GetReaderById(readerId);
            
            if (reader == null)
            {
                _consoleUIService.WriteLine($"Читатель с ID {readerId} не найден.");
                return true;
            }
            
            _consoleUIService.WriteLine($"\nИнформация о читателе (ID: {reader.Id}):");
            _consoleUIService.WriteLine($"ФИО: {reader.Fio}");
            _consoleUIService.WriteLine($"Email: {reader.Email}");
            _consoleUIService.WriteLine($"Телефон: {reader.Phone}");
            _consoleUIService.WriteLine($"Дата регистрации: {reader.RegisteredAt:dd.MM.yyyy HH:mm}");
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при получении информации о читателе: {ex.Message}");
        }
        
        return true;
    }
}