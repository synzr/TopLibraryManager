using TopLibraryManager.Services;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Commands;

public class UpdateReaderCommand : BaseCommand
{
    public UpdateReaderCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService,
        IBookService bookService,
        IReaderService readerService) 
        : base(consoleUIService, librarianService, bookService, readerService)
    {
    }
    
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Обновление информации о читателе ===");
        
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать ID читателя. Использование: обновитьчитателя <ID>");
            return true;
        }
        
        if (!int.TryParse(args[0], out int readerId))
        {
            _consoleUIService.WriteLine("Некорректный ID читателя. ID должен быть числом.");
            return true;
        }
        
        // Получаем текущую информацию о читателе
        var reader = _readerService.GetReaderById(readerId);
        if (reader == null)
        {
            _consoleUIService.WriteLine($"Читатель с ID {readerId} не найден.");
            return true;
        }
        
        _consoleUIService.WriteLine($"Текущая информация о читателе (ID: {reader.Id}):");
        _consoleUIService.WriteLine($"ФИО: {reader.Fio}");
        _consoleUIService.WriteLine($"Email: {reader.Email}");
        _consoleUIService.WriteLine($"Телефон: {reader.Phone}");
        _consoleUIService.WriteLine($"Дата регистрации: {reader.RegisteredAt:dd.MM.yyyy HH:mm}");
        
        _consoleUIService.WriteLine("\nВведите новые значения (оставьте пустым, чтобы не изменять):");
        
        string? newFio = _consoleUIService.ReadLine($"Новое ФИО [{reader.Fio}]: ");
        string? newEmail = _consoleUIService.ReadLine($"Новый email [{reader.Email}]: ");
        string? newPhone = _consoleUIService.ReadLine($"Новый телефон [{reader.Phone}]: ");
        
        try
        {
            var updatedReader = _readerService.UpdateReader(
                readerId,
                string.IsNullOrWhiteSpace(newFio) ? null : newFio,
                string.IsNullOrWhiteSpace(newEmail) ? null : newEmail,
                string.IsNullOrWhiteSpace(newPhone) ? null : newPhone);
            
            if (updatedReader == null)
            {
                _consoleUIService.WriteLine($"\nОшибка: читатель с ID {readerId} не найден.");
                return true;
            }
            
            _consoleUIService.WriteLine($"\nИнформация о читателе успешно обновлена:");
            _consoleUIService.WriteLine($"ФИО: {updatedReader.Fio}");
            _consoleUIService.WriteLine($"Email: {updatedReader.Email}");
            _consoleUIService.WriteLine($"Телефон: {updatedReader.Phone}");
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при обновлении читателя: {ex.Message}");
        }
        
        return true;
    }
}