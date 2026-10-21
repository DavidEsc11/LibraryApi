using LibraryApi.Data;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(LibraryDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email) =>
        await Set.FirstOrDefaultAsync(u => u.Email == email);
}
