using TopLibraryManager.Services;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Commands;

public class CreateReaderCommand : BaseCommand
{
    public CreateReaderCommand(
        IConsoleUIService consoleUIService, 
        ILibrarianService librarianService,
        IBookService bookService,
        IReaderService readerService) 
        : base(consoleUIService, librarianService, bookService, readerService)
    {
    }
    
    public override bool Execute(string[] args)
    {
        _consoleUIService.WriteLine("\n=== Добавление нового читателя ===");
        
        string? fio, email, phone;
        
        do 
        { 
            fio = _consoleUIService.ReadLine("ФИО читателя: "); 
        } 
        while (string.IsNullOrWhiteSpace(fio));
        
        do 
        { 
            email = _consoleUIService.ReadLine("Email: "); 
        } 
        while (string.IsNullOrWhiteSpace(email));
        
        do 
        { 
            phone = _consoleUIService.ReadLine("Телефон: "); 
        } 
        while (string.IsNullOrWhiteSpace(phone));
        
        try
        {
            var reader = _readerService.CreateReader(fio, email, phone);
            _consoleUIService.WriteLine($"\nЧитатель '{reader.Fio}' успешно добавлен (ID: {reader.Id}).");
            _consoleUIService.WriteLine($"Email: {reader.Email}, Телефон: {reader.Phone}");
            _consoleUIService.WriteLine($"Дата регистрации: {reader.RegisteredAt:dd.MM.yyyy HH:mm}");
        }
        catch (Exception ex)
        {
            _consoleUIService.WriteLine($"\nОшибка при добавлении читателя: {ex.Message}");
        }
        
        return true;
    }
}