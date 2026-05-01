using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Books;

public class DeleteBookCommand : ICommand
{
    public string Name => "удалитькнигу";
    public IEnumerable<string> Aliases => new[] { "deletebook" };
    public string Description => "Удаление книги по ID";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly IBookService _bookService;

    public DeleteBookCommand(IConsoleUIService consoleUIService, IBookService bookService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
    }
    
    public bool Execute(string[] args)
    {
        if (args.Length == 0)
        {
            _consoleUIService.WriteLine("Необходимо указать ID книги. Использование: удалитькнигу <ID>");
            _consoleUIService.WriteLine("Или используйте команду без параметров для интерактивного режима.");
            return true;
        }

        _consoleUIService.WriteLine("\n=== Удаление книги ===");
        
        if (!_bookService.AnyBooksExist())
        {
            _consoleUIService.WriteLine("В библиотеке нет книг для удаления.");
            return true;
        }
        
        if (!int.TryParse(args[0], out int bookId) || bookId <= 0)
        {
            _consoleUIService.WriteLine("Некорректный ID. Использование: удалитькнигу <ID>");
            return true;
        }
        
        var book = _bookService.GetBookById(bookId);
        if (book == null)
        {
            _consoleUIService.WriteLine($"Книга с ID {bookId} не найдена.");
            return true;
        }
        
        _consoleUIService.WriteLine($"Вы действительно хотите удалить книгу '{book.Title}' by {book.Author}? (да/нет)");
        var confirmation = _consoleUIService.ReadLine(null);
        
        if (confirmation?.ToLower() == "да")
        {
            var success = _bookService.DeleteBookById(bookId);
            if (success)
                _consoleUIService.WriteLine($"Книга '{book.Title}' успешно удалена.");
            else
                _consoleUIService.WriteLine("Не удалось удалить книгу.");
        }
        else
        {
            _consoleUIService.WriteLine("Удаление отменено.");
        }
        
        return true;
    }
}