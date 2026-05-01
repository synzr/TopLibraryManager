using Microsoft.EntityFrameworkCore;
using TopLibraryManager.Data;
using TopLibraryManager.Models.Entities;
using TopLibraryManager.Services.Interfaces;

namespace TopLibraryManager.Services.Implementations;

public class BookService(LibraryDbContext libraryDbContext) : IBookService
{
    /// <summary>
    /// Контекст базы данных библиотеки
    /// </summary>
    private readonly LibraryDbContext _libraryDbContext = libraryDbContext;
    
    /// <inheritdoc />
    public Book CreateBook(string title, string author, string genre, short year)
    {
        var book = new Book
        {
            Title = title,
            Author = author,
            Genre = genre,
            Year = year
        };
        
        _libraryDbContext.Books.Add(book);
        _libraryDbContext.SaveChanges();
        return book;
    }
    
    /// <inheritdoc />
    public Book? GetBookById(int id)
    {
        return _libraryDbContext.Books.Find(id);
    }
    
    /// <inheritdoc />
    public Book? UpdateBook(int id, string? title = null, string? author = null, string? genre = null, short? year = null)
    {
        var book = GetBookById(id);
        if (book == null)
            return null;
            
        if (title != null)
            book.Title = title;
        if (author != null)
            book.Author = author;
        if (genre != null)
            book.Genre = genre;
        if (year.HasValue)
            book.Year = year.Value;
            
        _libraryDbContext.SaveChanges();
        return book;
    }
    
    /// <inheritdoc />
    public bool DeleteBookById(int id)
    {
        var book = GetBookById(id);
        if (book == null)
            return false;
            
        _libraryDbContext.Books.Remove(book);
        _libraryDbContext.SaveChanges();
        return true;
    }
    
    /// <inheritdoc />
    public IEnumerable<Book> SearchBooks(string? title = null, string? author = null, string? genre = null, short? year = null)
    {
        var query = _libraryDbContext.Books.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(title))
            query = query.Where(b => b.Title.Contains(title));
        if (!string.IsNullOrWhiteSpace(author))
            query = query.Where(b => b.Author.Contains(author));
        if (!string.IsNullOrWhiteSpace(genre))
            query = query.Where(b => b.Genre.Contains(genre));
        if (year.HasValue)
            query = query.Where(b => b.Year == year.Value);
            
        return query.ToList();
    }
    
    /// <inheritdoc />
    public bool AnyBooksExist()
    {
        return _libraryDbContext.Books.Any();
    }
}