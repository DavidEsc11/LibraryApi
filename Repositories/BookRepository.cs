using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories;

public interface IBookRepository : IRepository<Book>
{
    // Libros de la biblioteca de un usuario.
    Task<List<Book>> GetByUserAsync(int userId);

    // Un libro solo se encuentra si pertenece a ese usuario.
    Task<Book?> GetByUserAndIdAsync(int userId, int bookId);
}

public class BookRepository : Repository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<List<Book>> GetByUserAsync(int userId) =>
        await Set.AsNoTracking().Where(b => b.UserId == userId).OrderBy(b => b.Title).ToListAsync();

    public async Task<Book?> GetByUserAndIdAsync(int userId, int bookId) =>
        await Set.FirstOrDefaultAsync(b => b.Id == bookId && b.UserId == userId);
}
