using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data;

public static class DatabaseInitializer
{
    // Crea la base de datos y las tablas. Reintenta porque SQL Server en Docker
    // tarda unos segundos en estar listo cuando arranca junto con la API.
    public static async Task InitializeAsync(IServiceProvider services, ILogger logger)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();

        const int maxAttempts = 30;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            try
            {
                await context.Database.EnsureCreatedAsync();
                logger.LogInformation("Base de datos lista.");
                return;
            }
            catch (Exception ex)
            {
                logger.LogWarning("Esperando a SQL Server (intento {Attempt}/{Max}): {Message}", attempt, maxAttempts, ex.Message);
                if (attempt == maxAttempts)
                {
                    throw;
                }
                await Task.Delay(TimeSpan.FromSeconds(3));
            }
        }
    }
}
