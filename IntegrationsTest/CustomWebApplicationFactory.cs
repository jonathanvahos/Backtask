using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions; // Necesario para RemoveAll
using ProyectoTareas.Data;

namespace IntegrationsTest
{
    public class CustomWebApplicationFactory<TProgram>
        : WebApplicationFactory<TProgram> where TProgram : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Establece el entorno de la aplicación como "Testing"
            builder.UseEnvironment("Testing");

            // Usa ConfigureTestServices para sobrescribir la configuración del Program.cs
            builder.ConfigureTestServices(services =>
            {
                // Elimina todas las registraciones de TareasDbContext y sus opciones (SQL Server)
                services.RemoveAll<TareasDbContext>();
                services.RemoveAll<DbContextOptions<TareasDbContext>>();

                // Registra el DbContext para usar InMemory Database en pruebas
                services.AddDbContext<TareasDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        }
    }
}




