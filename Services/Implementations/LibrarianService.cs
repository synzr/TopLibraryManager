using Microsoft.EntityFrameworkCore;
using TopLibraryManager.Data;
using TopLibraryManager.Models.Entities;
using TopLibraryManager.Utils;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Services.Implementations;

public class LibrarianService(LibraryDbContext libraryDbContext) : ILibrarianService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;
    
    /// <inheritdoc />
    public Librarian RegisterLibrarian(string fio, string login, string password)
    {
        var hashedPassword = PasswordHasher.HashPassword(password);
        
        var librarian = new Librarian
        {
            Fio = fio,
            Login = login,
            Password = hashedPassword
        };

        _libraryDbContext.Librarians.Add(librarian);
        _libraryDbContext.SaveChanges();
        
        return librarian;
    }
    
    /// <inheritdoc />
    public Librarian? GetLibrarianById(int id)
    {
        return _libraryDbContext.Librarians.Find(id);
    }
    
    /// <inheritdoc />
    public Librarian? GetLibrarianByLogin(string login)
    {
        return _libraryDbContext.Librarians
            .FirstOrDefault(l => l.Login == login);
    }
    
    /// <inheritdoc />
    public bool DeleteLibrarianById(int id)
    {
        var librarian = GetLibrarianById(id);
        if (librarian == null)
            return false;

        _libraryDbContext.Librarians.Remove(librarian);
        _libraryDbContext.SaveChanges();
        return true;
    }
    
    /// <inheritdoc />
    public bool DeleteLibrarianByLogin(string login)
    {
        var librarian = GetLibrarianByLogin(login);
        if (librarian == null)
            return false;

        _libraryDbContext.Librarians.Remove(librarian);
        _libraryDbContext.SaveChanges();
        return true;
    }
    
    /// <inheritdoc />
    public bool AnyLibrarianExists()
    {
        return _libraryDbContext.Librarians.Any();
    }
    
    /// <inheritdoc />
    public Librarian? Authenticate(string login, string password)
    {
        var librarian = GetLibrarianByLogin(login);
        if (librarian == null)
            return null;
            
        if (!PasswordHasher.VerifyPassword(password, librarian.Password))
            return null;
            
        return librarian;
    }
}