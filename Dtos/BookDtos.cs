using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos;

// Datos para añadir o editar un libro de la colección.
public class BookInputDto
{
    [Required, StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required, StringLength(200)]
    public string Author { get; set; } = string.Empty;

    [Range(1, 2100)]
    public int PublicationYear { get; set; }

    // Opcional: URL de la imagen de portada.
    [StringLength(500)]
    public string? CoverImageUrl { get; set; }
}

// Datos para calificar un libro (1 a 5 estrellas) y escribir una reseña.
public class RateBookDto
{
    [Range(1, 5)]
    public int Rating { get; set; }

    [StringLength(2000)]
    public string? Review { get; set; }
}

public class BookDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public string? CoverImageUrl { get; set; }
    public int? Rating { get; set; }
    public string? Review { get; set; }
}
