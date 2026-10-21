using LibraryApi.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly LibraryDbContext Context;
    protected readonly DbSet<T> Set;

    public Repository(LibraryDbContext context)
    {
        Context = context;
        Set = context.Set<T>();
    }

    public async Task<List<T>> GetAllAsync() => await Set.AsNoTracking().ToListAsync();

    public async Task<T?> GetByIdAsync(int id) => await Set.FindAsync(id);

    public async Task AddAsync(T entity) => await Set.AddAsync(entity);

    public void Update(T entity) => Set.Update(entity);

    public void Delete(T entity) => Set.Remove(entity);

    public async Task SaveChangesAsync() => await Context.SaveChangesAsync();
}
