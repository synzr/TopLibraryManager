using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class SearchBooksCommand : BaseCommand
{
    public SearchBooksCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService,
        IReaderService readerService)
        : base(consoleUIService, librarianService, bookService, readerService)
    {
    }
    
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Поиск книг ===");
        
        string? title = null;
        string? author = null;
        string? genre = null;
        short? year = null;
        
        // Если переданы аргументы, используем первый как поиск по названию
        if (args.Length > 0)
        {
            title = args[0];
            _consoleUIService.WriteLine($"Поиск по названию: '{title}'");
            _consoleUIService.WriteLine("(оставьте поля пустыми для поиска только по названию)");
        }
        else
        {
            _consoleUIService.WriteLine("Введите критерии поиска (оставьте пустым для пропуска):");
        }
        
        if (args.Length == 0 || args.Length > 1)
        {
            // Интерактивный режим или дополнительные параметры
            if (args.Length == 0)
            {
                title = _consoleUIService.ReadLine("Название: ");
            }
            
            author = _consoleUIService.ReadLine("Автор: ");
            genre = _consoleUIService.ReadLine("Жанр: ");
            
            while (true)
            {
                var yearInput = _consoleUIService.ReadLine("Год издания: ");
                if (string.IsNullOrWhiteSpace(yearInput))
                    break;
                if (short.TryParse(yearInput, out short parsedYear) && parsedYear > 0)
                {
                    year = parsedYear;
                    break;
                }
                _consoleUIService.WriteLine("Некорректный год. Попробуйте снова или оставьте пустым.");
            }
        }
        
        var books = _bookService.SearchBooks(
            string.IsNullOrWhiteSpace(title) ? null : title,
            string.IsNullOrWhiteSpace(author) ? null : author,
            string.IsNullOrWhiteSpace(genre) ? null : genre,
            year
        ).ToList();
        
        if (!books.Any())
        {
            _consoleUIService.WriteLine("\nКниги по заданным критериям не найдены.");
            return true;
        }
        
        _consoleUIService.WriteLine($"\nНайдено книг: {books.Count}");
        _consoleUIService.WriteLine("=========================================");
        
        foreach (var book in books)
        {
            _consoleUIService.WriteLine($"ID: {book.Id}");
            _consoleUIService.WriteLine($"  Название: {book.Title}");
            _consoleUIService.WriteLine($"  Автор: {book.Author}");
            _consoleUIService.WriteLine($"  Жанр: {book.Genre}");
            _consoleUIService.WriteLine($"  Год: {book.Year}");
            _consoleUIService.WriteLine("-----------------------------------------");
        }
        
        return true;
    }
}
