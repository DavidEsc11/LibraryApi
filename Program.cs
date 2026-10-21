using LibraryApi.Data;
using LibraryApi.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Entity Framework Core. La cadena de conexión viene de appsettings.json o de la variable
// de entorno ConnectionStrings__DefaultConnection (la usa docker-compose).
// - Con "Server=..." usa SQL Server (modo Docker).
// - Con "Data Source=..." usa una base de datos local SQLite (modo sin Docker).
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? "Data Source=library.db";

builder.Services.AddDbContext<LibraryDbContext>(options =>
{
    if (connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase))
    {
        options.UseSqlServer(connectionString);
    }
    else
    {
        options.UseSqlite(connectionString);
    }
});

// Patrón Repository.
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

var app = builder.Build();

await DatabaseInitializer.InitializeAsync(app.Services, app.Logger);

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/", () => Results.Redirect("/swagger"));
app.MapControllers();

app.Run();
