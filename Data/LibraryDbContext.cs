using Microsoft.EntityFrameworkCore;

namespace TopLibraryManager.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }
}
