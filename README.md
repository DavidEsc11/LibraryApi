# Librería Digital – API

API web con ASP.NET Core (.NET 10) para gestionar la colección de libros de cada usuario.

## Tecnologías
- ASP.NET Core Web API
- Entity Framework Core + SQL Server
- Patrón Repository (`IRepository<T>`, `IUserRepository`, `IBookRepository`)
- Swagger (documentación y pruebas)
- Docker y docker-compose (API + SQL Server)

## Modelos
- **User**: nombre, apellido, correo (único) y contraseña (se guarda con hash).
- **Book**: título, autor, año de publicación, portada (opcional), calificación (1 a 5) y reseña. Cada libro pertenece a un usuario.

## Cómo ejecutarla

**Sin Docker (base de datos local SQLite):**
```
dotnet run --urls http://localhost:5000
```
Luego abre http://localhost:5000/swagger

**Con Docker (API + SQL Server):**
```
docker compose up --build
```
Luego abre http://localhost:8080/swagger

La API elige la base de datos según la cadena de conexión: `Server=...` usa SQL Server y `Data Source=...` usa SQLite.

## Endpoints
| Método | Ruta | Descripción |
|---|---|---|
| POST | /api/users | Registrar usuario |
| GET | /api/users | Listar usuarios |
| GET | /api/users/{id} | Consultar usuario |
| PUT | /api/users/{id} | Editar usuario |
| DELETE | /api/users/{id} | Eliminar usuario (y su biblioteca) |
| GET | /api/users/{userId}/books | Listar libros del usuario |
| POST | /api/users/{userId}/books | Añadir libro |
| GET | /api/users/{userId}/books/{bookId} | Consultar libro |
| PUT | /api/users/{userId}/books/{bookId} | Editar libro |
| PUT | /api/users/{userId}/books/{bookId}/rating | Calificar y reseñar |
| DELETE | /api/users/{userId}/books/{bookId} | Eliminar libro |
