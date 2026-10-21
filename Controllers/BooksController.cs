using LibraryApi.Dtos;
using LibraryApi.Models;
using LibraryApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

// Cada usuario tiene su propia biblioteca: todas las rutas cuelgan de /api/users/{userId}/books
[ApiController]
[Route("api/users/{userId:int}/books")]
public class BooksController : ControllerBase
{
    private readonly IBookRepository _books;
    private readonly IUserRepository _users;

    public BooksController(IBookRepository books, IUserRepository users)
    {
        _books = books;
        _users = users;
    }

    // Listar los libros de la colección del usuario.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<BookDto>>> GetAll(int userId)
    {
        if (await _users.GetByIdAsync(userId) is null)
        {
            return NotFound("Usuario no encontrado.");
        }

        var books = await _books.GetByUserAsync(userId);
        return Ok(books.Select(ToDto));
    }

    // Consultar un libro de la colección.
    [HttpGet("{bookId:int}")]
    public async Task<ActionResult<BookDto>> GetById(int userId, int bookId)
    {
        var book = await _books.GetByUserAndIdAsync(userId, bookId);
        return book is null ? NotFound() : Ok(ToDto(book));
    }

    // Añadir un libro a la colección.
    [HttpPost]
    public async Task<ActionResult<BookDto>> Add(int userId, BookInputDto dto)
    {
        if (await _users.GetByIdAsync(userId) is null)
        {
            return NotFound("Usuario no encontrado.");
        }

        var book = new Book
        {
            UserId = userId,
            Title = dto.Title.Trim(),
            Author = dto.Author.Trim(),
            PublicationYear = dto.PublicationYear,
            CoverImageUrl = string.IsNullOrWhiteSpace(dto.CoverImageUrl) ? null : dto.CoverImageUrl.Trim(),
        };

        await _books.AddAsync(book);
        await _books.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { userId, bookId = book.Id }, ToDto(book));
    }

    // Editar un libro de la colección.
    [HttpPut("{bookId:int}")]
    public async Task<ActionResult<BookDto>> Update(int userId, int bookId, BookInputDto dto)
    {
        var book = await _books.GetByUserAndIdAsync(userId, bookId);
        if (book is null)
        {
            return NotFound();
        }

        book.Title = dto.Title.Trim();
        book.Author = dto.Author.Trim();
        book.PublicationYear = dto.PublicationYear;
        book.CoverImageUrl = string.IsNullOrWhiteSpace(dto.CoverImageUrl) ? null : dto.CoverImageUrl.Trim();

        await _books.SaveChangesAsync();
        return Ok(ToDto(book));
    }

    // Calificar el libro (1 a 5 estrellas) y escribir una reseña.
    [HttpPut("{bookId:int}/rating")]
    public async Task<ActionResult<BookDto>> Rate(int userId, int bookId, RateBookDto dto)
    {
        var book = await _books.GetByUserAndIdAsync(userId, bookId);
        if (book is null)
        {
            return NotFound();
        }

        book.Rating = dto.Rating;
        book.Review = string.IsNullOrWhiteSpace(dto.Review) ? null : dto.Review.Trim();

        await _books.SaveChangesAsync();
        return Ok(ToDto(book));
    }

    // Eliminar un libro de la colección.
    [HttpDelete("{bookId:int}")]
    public async Task<IActionResult> Delete(int userId, int bookId)
    {
        var book = await _books.GetByUserAndIdAsync(userId, bookId);
        if (book is null)
        {
            return NotFound();
        }

        _books.Delete(book);
        await _books.SaveChangesAsync();
        return NoContent();
    }

    private static BookDto ToDto(Book book) => new()
    {
        Id = book.Id,
        UserId = book.UserId,
        Title = book.Title,
        Author = book.Author,
        PublicationYear = book.PublicationYear,
        CoverImageUrl = book.CoverImageUrl,
        Rating = book.Rating,
        Review = book.Review,
    };
}
