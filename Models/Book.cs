namespace LibraryApi.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? CoverImageUrl { get; set; }

    // Calificación de 1 a 5 y reseña escrita (ambas opcionales hasta que el usuario las agregue).
    public int? Rating { get; set; }
    public string? Review { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}
