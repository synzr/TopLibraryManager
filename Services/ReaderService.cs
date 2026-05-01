using Microsoft.EntityFrameworkCore;
using TopLibraryManager.Data;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Services;

public class ReaderService(LibraryDbContext libraryDbContext) : IReaderService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;
    
    /// <inheritdoc />
    public Reader CreateReader(string fio, string email, string phone)
    {
        if (string.IsNullOrWhiteSpace(fio))
            throw new ArgumentException("ФИО не может быть пустым", nameof(fio));
        
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email не может быть пустым", nameof(email));
        
        if (string.IsNullOrWhiteSpace(phone))
            throw new ArgumentException("Телефон не может быть пустым", nameof(phone));
        
        var reader = new Reader
        {
            Fio = fio,
            Email = email,
            Phone = phone,
            RegisteredAt = DateTime.Now
        };
        
        _libraryDbContext.Readers.Add(reader);
        _libraryDbContext.SaveChanges();
        return reader;
    }
    
    /// <inheritdoc />
    public Reader? GetReaderById(int id)
    {
        return _libraryDbContext.Readers.Find(id);
    }
    
    /// <inheritdoc />
    public Reader? UpdateReader(int id, string? fio = null, string? email = null, string? phone = null)
    {
        var reader = GetReaderById(id);
        if (reader == null)
            return null;
            
        if (fio != null)
            reader.Fio = fio;
        if (email != null)
            reader.Email = email;
        if (phone != null)
            reader.Phone = phone;
            
        _libraryDbContext.SaveChanges();
        return reader;
    }
    
    /// <inheritdoc />
    public bool DeleteReaderById(int id)
    {
        var reader = GetReaderById(id);
        if (reader == null)
            return false;
            
        _libraryDbContext.Readers.Remove(reader);
        _libraryDbContext.SaveChanges();
        return true;
    }
    
    /// <inheritdoc />
    public IEnumerable<Reader> SearchReaders(string? fio = null, string? email = null, string? phone = null)
    {
        var query = _libraryDbContext.Readers.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(fio))
            query = query.Where(r => r.Fio.Contains(fio));
        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(r => r.Email.Contains(email));
        if (!string.IsNullOrWhiteSpace(phone))
            query = query.Where(r => r.Phone.Contains(phone));
            
        return query.ToList();
    }
    
    /// <inheritdoc />
    public bool AnyReadersExist()
    {
        return _libraryDbContext.Readers.Any();
    }
}