using System.Collections.Generic;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Commands.Loans;

public class CreateLoanCommand : ICommand
{
    public string Name => "новаявыдача";
    public IEnumerable<string> Aliases => new[] { "выдатькнигу", "createloan" };
    public string Description => "Создание новой выдачи книги читателю";
    
    private readonly IConsoleUIService _consoleUIService;
    private readonly ILoanService _loanService;
    private readonly IBookService _bookService;
    private readonly IReaderService _readerService;
    private readonly ILibrarianService _librarianService;

    public CreateLoanCommand(
        IConsoleUIService consoleUIService,
        ILoanService loanService,
        IBookService bookService,
        IReaderService readerService,
        ILibrarianService librarianService)
    {
        _consoleUIService = consoleUIService ?? throw new ArgumentNullException(nameof(consoleUIService));
        _loanService = loanService ?? throw new ArgumentNullException(nameof(loanService));
        _bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        _readerService = readerService ?? throw new ArgumentNullException(nameof(readerService));
        _librarianService = librarianService ?? throw new ArgumentNullException(nameof(librarianService));
    }
    
    public bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Создание новой выдачи книги ===");
        
        // Проверка наличия необходимых данных
        if (!_bookService.AnyBooksExist())
        {
            _consoleUIService.WriteLine("Ошибка: В системе нет книг. Сначала добавьте книги.");
            return true;
        }
        
        if (!_readerService.AnyReadersExist())
        {
            _consoleUIService.WriteLine("Ошибка: В системе нет читателей. Сначала зарегистрируйте читателей.");
            return true;
        }
        
        // Получение ID книги
        int bookId;
        while (true)
        {
            var bookIdInput = _consoleUIService.ReadLine("ID книги: ");
            if (int.TryParse(bookIdInput, out bookId) && bookId > 0)
            {
                var book = _bookService.GetBookById(bookId);
                if (book != null)
                {
                    _consoleUIService.WriteLine($"Книга: {book.Title} ({book.Author}, {book.Year})");
                    break;
                }
                _consoleUIService.WriteLine("Книга с таким ID не найдена. Попробуйте снова.");
            }
            else
            {
                _consoleUIService.WriteLine("Некорректный ID книги. Попробуйте снова.");
            }
        }
        
        // Получение ID читателя
        int readerId;
        while (true)
        {
            var readerIdInput = _consoleUIService.ReadLine("ID читателя: ");
            if (int.TryParse(readerIdInput, out readerId) && readerId > 0)
            {
                var reader = _readerService.GetReaderById(readerId);
                if (reader != null)
                {
                    _consoleUIService.WriteLine($"Читатель: {reader.Fio}");
                    break;
                }
                _consoleUIService.WriteLine("Читатель с таким ID не найден. Попробуйте снова.");
            }
            else
            {
                _consoleUIService.WriteLine("Некорректный ID читателя. Попробуйте снова.");
            }
        }
        
        // Получение ID библиотекаря (в реальной системе это может быть текущий авторизованный библиотекарь)
        int librarianId;
        while (true)
        {
            var librarianIdInput = _consoleUIService.ReadLine("ID библиотекаря: ");
            if (int.TryParse(librarianIdInput, out librarianId) && librarianId > 0)
            {
                var librarian = _librarianService.GetLibrarianById(librarianId);
                if (librarian != null)
                {
                    _consoleUIService.WriteLine($"Библиотекарь: {librarian.Fio}");
                    break;
                }
                _consoleUIService.WriteLine("Библиотекарь с таким ID не найден. Попробуйте снова.");
            }
            else
            {
                _consoleUIService.WriteLine("Некорректный ID библиотекаря. Попробуйте снова.");
            }
        }
        
        // Получение даты возврата
        DateOnly returnAt;
        while (true)
        {
            var returnAtInput = _consoleUIService.ReadLine("Дата возврата (дд.мм.гггг): ");
            if (DateOnly.TryParse(returnAtInput, out returnAt) && returnAt > DateOnly.FromDateTime(DateTime.Today))
            {
                break;
            }
            _consoleUIService.WriteLine("Некорректная дата. Дата должна быть в будущем. Попробуйте снова.");
        }
        
        try
        {
            var loan = _loanService.CreateLoan(bookId, readerId, librarianId, returnAt);
            _consoleUIService.WriteLine($"\nВыдача успешно создана (ID: {loan.Id}).");
            _consoleUIService.WriteLine($"Книга должна быть возвращена до: {loan.ReturnAt:dd.MM.yyyy}");
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при создании выдачи: {ex.Message}");
        }
        
        return true;
    }
}