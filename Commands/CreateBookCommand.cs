using TopLibraryManager.Services;

namespace TopLibraryManager.Commands;

public class CreateBookCommand : BaseCommand
{
    public CreateBookCommand(
        IConsoleUIService consoleUIService,
        ILibrarianService librarianService,
        IBookService bookService,
        IReaderService readerService)
        : base(consoleUIService, librarianService, bookService, readerService)
    {
    }
    
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Добавление новой книги ===");
        
        string? title, author, genre;
        short year;
        
        do 
        { 
            title = _consoleUIService.ReadLine("Название книги: ");
        } 
        while (string.IsNullOrWhiteSpace(title));
        
        do 
        { 
            author = _consoleUIService.ReadLine("Автор: ");
        } 
        while (string.IsNullOrWhiteSpace(author));
        
        do 
        { 
            genre = _consoleUIService.ReadLine("Жанр: ");
        } 
        while (string.IsNullOrWhiteSpace(genre));
        
        while (true)
        {
            var yearInput = _consoleUIService.ReadLine("Год издания: ");
            if (short.TryParse(yearInput, out year) && year > 0 && year <= DateTime.Now.Year + 1)
                break;
            _consoleUIService.WriteLine("Некорректный год. Попробуйте снова.");
        }
        
        try
        {
            var book = _bookService.CreateBook(title, author, genre, year);
            _consoleUIService.WriteLine($"\nКнига '{book.Title}' успешно добавлена (ID: {book.Id}).");
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при добавлении книги: {ex.Message}");
        }
        
        return true;
    }
}
