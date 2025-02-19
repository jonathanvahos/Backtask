using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProyectoTareas.Data;
using ProyectoTareas.Models;
using Xunit;

namespace IntegrationsTest
{
    public class TareasControllerIntegrationTests
        : IClassFixture<CustomWebApplicationFactory<ProyectoTareas.Program>>
    {
        private readonly HttpClient _client;
        private readonly CustomWebApplicationFactory<ProyectoTareas.Program> _factory;

        public TareasControllerIntegrationTests(CustomWebApplicationFactory<ProyectoTareas.Program> factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetTareas_DevuelveListaTareas()
        {
            // Arrange: Crear datos de prueba
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TareasDbContext>();
                dbContext.Tareas.Add(new Tarea("Test 1", "Descripción 1", false));
                dbContext.Tareas.Add(new Tarea("Test 2", "Descripción 2", true));
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync("/api/tareas");

            // Assert
            response.EnsureSuccessStatusCode();
            var tareas = await response.Content.ReadFromJsonAsync<List<TareaDTO>>();
            Assert.NotNull(tareas);
            Assert.Equal(2, tareas.Count);
        }

        [Fact]
        public async Task PostTarea_CreaTareaCorrectamente()
        {
            // Arrange
            var nuevaTarea = new TareaDTO
            {
                Titulo = "Nueva Tarea",
                Descripcion = "Descripción de prueba",
                Completada = false
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/tareas", nuevaTarea);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var tareaCreada = await response.Content.ReadFromJsonAsync<TareaDTO>();
            Assert.NotNull(tareaCreada);
            Assert.Equal(nuevaTarea.Titulo, tareaCreada.Titulo);

            // Verificar en BD
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TareasDbContext>();
                var tareaEnBD = await dbContext.Tareas.FindAsync(tareaCreada!.Id);
                Assert.NotNull(tareaEnBD);
            }
        }

        [Fact]
        public async Task GetTareaPorId_DevuelveTareaCorrecta()
        {
            // Arrange
            var tarea = new Tarea("Test ID", "Descripción", false);
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TareasDbContext>();
                dbContext.Tareas.Add(tarea);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.GetAsync($"/api/tareas/{tarea.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var tareaObtenida = await response.Content.ReadFromJsonAsync<TareaDTO>();
            Assert.NotNull(tareaObtenida);
            Assert.Equal(tarea.Id, tareaObtenida!.Id);
        }

        [Fact]
        public async Task DeleteTarea_EliminaCorrectamente()
        {
            // Arrange
            var tarea = new Tarea("Eliminar", "Descripción", false);
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TareasDbContext>();
                dbContext.Tareas.Add(tarea);
                await dbContext.SaveChangesAsync();
            }

            // Act
            var response = await _client.DeleteAsync($"/api/tareas/{tarea.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verificar eliminación
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TareasDbContext>();
                var tareaEliminada = await dbContext.Tareas.FindAsync(tarea.Id);
                Assert.Null(tareaEliminada);
            }
        }

        [Fact]
        public async Task PutTarea_ActualizaCorrectamente()
        {
            // Arrange
            var tareaOriginal = new Tarea("Original", "Descripción", false);
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TareasDbContext>();
                dbContext.Tareas.Add(tareaOriginal);
                await dbContext.SaveChangesAsync();
            }

            var tareaActualizada = new TareaDTO
            {
                Id = tareaOriginal.Id,
                Titulo = "Actualizado",
                Descripcion = "Nueva descripción",
                Completada = true
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/tareas/{tareaOriginal.Id}", tareaActualizada);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Verificar cambios
            using (var scope = _factory.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TareasDbContext>();
                var tareaEnBD = await dbContext.Tareas.FindAsync(tareaOriginal.Id);

                Assert.NotNull(tareaEnBD);
                Assert.Equal(tareaActualizada.Titulo, tareaEnBD!.Titulo);
                Assert.Equal(tareaActualizada.Completada, tareaEnBD.Completada);
            }
        }
    }
}
