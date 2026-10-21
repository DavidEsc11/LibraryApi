using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Dtos;

// Datos para registrar un usuario.
public class CreateUserDto
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;
}

// Datos para editar un usuario. La contraseña es opcional: si no se envía, no cambia.
public class UpdateUserDto
{
    [Required, StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [MinLength(6)]
    public string? Password { get; set; }
}

// Lo que devuelve la API: nunca incluye la contraseña.
public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}
