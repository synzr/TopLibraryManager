using TopLibraryManager.Commands;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Readers;

public class SearchReadersCommand : ICommand
{
    private readonly IConsoleUIService _consoleUIService;
    private readonly IReaderService _readerService;

    public SearchReadersCommand(
        IConsoleUIService consoleUIService,
        IReaderService readerService)
    {
        _consoleUIService = consoleUIService;
        _readerService = readerService;
    }
    
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Поиск читателей ===");
        
        string? fioFilter = null;
        string? emailFilter = null;
        string? phoneFilter = null;
        
        // Если аргументы переданы в командной строке
        if (args.Length > 0)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("--фио", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    fioFilter = args[++i];
                }
                else if (args[i].Equals("--email", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    emailFilter = args[++i];
                }
                else if (args[i].Equals("--телефон", StringComparison.OrdinalIgnoreCase) && i + 1 < args.Length)
                {
                    phoneFilter = args[++i];
                }
                else if (args[i].Equals("--помощь", StringComparison.OrdinalIgnoreCase))
                {
                    ShowHelp();
                    return true;
                }
            }
        }
        
        // Если фильтры не указаны в аргументах, запрашиваем интерактивно
        if (fioFilter == null && emailFilter == null && phoneFilter == null)
        {
            _consoleUIService.WriteLine("Введите критерии поиска (оставьте пустым, чтобы пропустить):");
            
            fioFilter = _consoleUIService.ReadLine("ФИО (частичное совпадение): ");
            emailFilter = _consoleUIService.ReadLine("Email (частичное совпадение): ");
            phoneFilter = _consoleUIService.ReadLine("Телефон (частичное совпадение): ");
            
            // Если все поля пустые, ищем всех читателей
            if (string.IsNullOrWhiteSpace(fioFilter) && 
                string.IsNullOrWhiteSpace(emailFilter) && 
                string.IsNullOrWhiteSpace(phoneFilter))
            {
                _consoleUIService.WriteLine("Поиск всех читателей...");
            }
        }
        
        try
        {
            var readers = _readerService.SearchReaders(
                string.IsNullOrWhiteSpace(fioFilter) ? null : fioFilter,
                string.IsNullOrWhiteSpace(emailFilter) ? null : emailFilter,
                string.IsNullOrWhiteSpace(phoneFilter) ? null : phoneFilter);
            
            var readersList = readers.ToList();
            
            if (!readersList.Any())
            {
                _consoleUIService.WriteLine("\nЧитатели не найдены.");
                return true;
            }
            
            _consoleUIService.WriteLine($"\nНайдено читателей: {readersList.Count}");
            _consoleUIService.WriteLine("=========================================");
            
            foreach (var reader in readersList)
            {
                _consoleUIService.WriteLine($"ID: {reader.Id}");
                _consoleUIService.WriteLine($"ФИО: {reader.Fio}");
                _consoleUIService.WriteLine($"Email: {reader.Email}");
                _consoleUIService.WriteLine($"Телефон: {reader.Phone}");
                _consoleUIService.WriteLine($"Дата регистрации: {reader.RegisteredAt:dd.MM.yyyy HH:mm}");
                _consoleUIService.WriteLine("-----------------------------------------");
            }
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при поиске читателей: {ex.Message}");
        }
        
        return true;
    }
    
    private void ShowHelp()
    {
        _consoleUIService.WriteLine("\nИспользование команды поиска читателей:");
        _consoleUIService.WriteLine("  найтичитателей [--фио <значение>] [--email <значение>] [--телефон <значение>]");
        _consoleUIService.WriteLine("  найтичитателей --помощь");
        _consoleUIService.WriteLine("\nПримеры:");
        _consoleUIService.WriteLine("  найтичитателей --фио Иванов");
        _consoleUIService.WriteLine("  найтичитателей --email gmail.com --телефон 912");
        _consoleUIService.WriteLine("\nЕсли аргументы не указаны, будет запущен интерактивный режим.");
    }
}