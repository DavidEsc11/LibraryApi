using LibraryApi.Dtos;
using LibraryApi.Models;
using LibraryApi.Repositories;
using LibraryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _users;

    public UsersController(IUserRepository users)
    {
        _users = users;
    }

    // Registrar un usuario.
    [HttpPost]
    public async Task<ActionResult<UserDto>> Register(CreateUserDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        if (await _users.GetByEmailAsync(email) is not null)
        {
            return Conflict("Ya existe un usuario con ese correo.");
        }

        var user = new User
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim(),
            Email = email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
        };

        await _users.AddAsync(user);
        await _users.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.Id }, ToDto(user));
    }

    // Listar usuarios.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _users.GetAllAsync();
        return Ok(users.Select(ToDto));
    }

    // Consultar un usuario.
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _users.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(ToDto(user));
    }

    // Editar un usuario.
    [HttpPut("{id:int}")]
    public async Task<ActionResult<UserDto>> Update(int id, UpdateUserDto dto)
    {
        var user = await _users.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        var email = dto.Email.Trim().ToLowerInvariant();
        var existing = await _users.GetByEmailAsync(email);
        if (existing is not null && existing.Id != id)
        {
            return Conflict("Ya existe otro usuario con ese correo.");
        }

        user.FirstName = dto.FirstName.Trim();
        user.LastName = dto.LastName.Trim();
        user.Email = email;
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            user.PasswordHash = PasswordHasher.Hash(dto.Password);
        }

        await _users.SaveChangesAsync();
        return Ok(ToDto(user));
    }

    // Eliminar un usuario (también se elimina su biblioteca).
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _users.GetByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        _users.Delete(user);
        await _users.SaveChangesAsync();
        return NoContent();
    }

    private static UserDto ToDto(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
    };
}
