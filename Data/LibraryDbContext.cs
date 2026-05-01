using Microsoft.EntityFrameworkCore;
using TopLibraryManager.Models.Entities;

namespace TopLibraryManager.Data;

public class LibraryDbContext : DbContext
{
    /// <summary>
    /// Библиотекари
    /// </summary>
    public DbSet<Librarian> Librarians { get; set; }

    /// <summary>
    /// Книги
    /// </summary>
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// Читатели
    /// </summary>
    public DbSet<Reader> Readers { get; set; }

    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }
}
