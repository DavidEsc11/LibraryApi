using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
            entity.Property(b => b.Author).IsRequired().HasMaxLength(200);
            entity.Property(b => b.CoverImageUrl).HasMaxLength(500);
            entity.Property(b => b.Review).HasMaxLength(2000);

            // Un usuario tiene muchos libros; al borrar el usuario se borra su biblioteca.
            entity.HasOne(b => b.User)
                  .WithMany(u => u.Books)
                  .HasForeignKey(b => b.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
