using System;
using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Books;

public class UpdateBookCommand : ICommand
{
    public string Name => "изменитькнигу";
    public IEnumerable<string> Aliases => new[] { "обновитькнигу", "updatebook" };
    public string Description => "Обновление информации о книге по ID";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly IBookService _bookService;

    public UpdateBookCommand(IConsoleUIService consoleUIService, IBookService bookService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
    }
    
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Обновление информации о книге ===");
        
        if (!_bookService.AnyBooksExist())
        {
            _consoleUIService.WriteLine("В библиотеке нет книг для обновления.");
            return true;
        }
        
        int bookId;
        
        // Если передан ID как параметр
        if (args.Length > 0)
        {
            if (!int.TryParse(args[0], out bookId) || bookId <= 0)
            {
                _consoleUIService.WriteLine("Некорректный ID. Использование: обновитькнигу <ID>");
                return true;
            }
        }
        else
        {
            // Интерактивный ввод ID
            while (true)
            {
                var idInput = _consoleUIService.ReadLine("ID книги для обновления: ");
                if (int.TryParse(idInput, out bookId) && bookId > 0)
                    break;
                _consoleUIService.WriteLine("Некорректный ID. Попробуйте снова.");
            }
        }
        
        var book = _bookService.GetBookById(bookId);
        if (book == null)
        {
            _consoleUIService.WriteLine($"Книга с ID {bookId} не найдена.");
            return true;
        }
        
        _consoleUIService.WriteLine($"Текущая информация: '{book.Title}' by {book.Author}, {book.Genre}, {book.Year}");
        _consoleUIService.WriteLine("Оставьте поле пустым, чтобы не изменять значение.");
        
        var newTitle = _consoleUIService.ReadLine($"Новое название [{book.Title}]: ");
        var newAuthor = _consoleUIService.ReadLine($"Новый автор [{book.Author}]: ");
        var newGenre = _consoleUIService.ReadLine($"Новый жанр [{book.Genre}]: ");
        
        short? newYear = null;
        while (true)
        {
            var yearInput = _consoleUIService.ReadLine($"Новый год издания [{book.Year}]: ");
            if (string.IsNullOrWhiteSpace(yearInput))
                break;
            if (short.TryParse(yearInput, out short parsedYear) && parsedYear > 0 && parsedYear <= DateTime.Now.Year + 1)
            {
                newYear = parsedYear;
                break;
            }
            _consoleUIService.WriteLine("Некорректный год. Попробуйте снова.");
        }
        
        try
        {
            var updatedBook = _bookService.UpdateBook(
                bookId,
                string.IsNullOrWhiteSpace(newTitle) ? null : newTitle,
                string.IsNullOrWhiteSpace(newAuthor) ? null : newAuthor,
                string.IsNullOrWhiteSpace(newGenre) ? null : newGenre,
                newYear
            );
            
            if (updatedBook != null)
                _consoleUIService.WriteLine($"\nКнига успешно обновлена: '{updatedBook.Title}' (ID: {updatedBook.Id}).");
            else
                _consoleUIService.WriteLine("\nНе удалось обновить книгу.");
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при обновлении книги: {ex.Message}");
        }
        
        return true;
    }
}