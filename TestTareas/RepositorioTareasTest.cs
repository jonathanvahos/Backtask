
using Microsoft.EntityFrameworkCore;
using ProyectoTareas.Data;
using ProyectoTareas.Models;
using System;
using Xunit;

namespace TestTareas
{
    public class RepositorioTareasTest : IDisposable
    {
        private readonly TareasDbContext _context;
        private readonly RepositorioTareas _repositorio;

        public RepositorioTareasTest()
        {
            // Configurar la base de datos en memoria con un nombre único por prueba
            var options = new DbContextOptionsBuilder<TareasDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new TareasDbContext(options);
            _repositorio = new RepositorioTareas(_context);
        }

        // Limpiar la base de datos en memoria después de cada prueba
        public void Dispose()
        {
            _context.Database.EnsureDeleted(); // Elimina la base de datos en memoria
            _context.Dispose(); // Libera recursos
        }

        [Fact]
        public async Task AgregarTarea()
        {
            // Arrange: Crear una nueva tarea
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false);

            // Act: Agregar la tarea y buscarla en la base de datos
            await _repositorio.AgregarAsync(tarea);
            var tareaAgregada = await _repositorio.BuscarPorIdAsync(tarea.Id);

            // Assert: Verificar que la tarea fue agregada correctamente
            Assert.NotNull(tareaAgregada);
            Assert.Equal(tarea.MostrarTarea(), tareaAgregada.MostrarTarea());
        }

        [Fact]
        public async Task ActualizarTarea()
        {
            // Arrange: Crear y agregar una nueva tarea
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false);
            await _repositorio.AgregarAsync(tarea);

            // Act: Modificar los valores de la tarea y actualizarla en la base de datos
            tarea.Titulo = "Tarea 1 Actualizada";
            tarea.Descripcion = "Descripcion de la tarea 1 Actualizada";
            tarea.Completada = true;
            await _repositorio.ActualizarAsync(tarea);

            var tareaActualizada = await _repositorio.BuscarPorIdAsync(tarea.Id);

            // Assert: Verificar que la tarea fue actualizada correctamente
            Assert.NotNull(tareaActualizada);
            Assert.Equal(tarea.MostrarTarea(), tareaActualizada.MostrarTarea());
        }

        [Fact]
        public async Task EliminarTarea()
        {
            // Arrange: Crear y agregar una nueva tarea
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false);
            await _repositorio.AgregarAsync(tarea);

            // Act: Eliminar la tarea y buscarla en la base de datos
            await _repositorio.EliminarAsync(tarea.Id);
            var tareaEliminada = await _repositorio.BuscarPorIdAsync(tarea.Id);

            // Assert: Verificar que la tarea fue eliminada correctamente
            Assert.Null(tareaEliminada);
        }

        [Fact]
        public async Task ObtenerTodasLasTareas()
        {
            // Arrange: Crear y agregar varias tareas
            var tarea1 = new Tarea("Tarea 1", "Descripcion de la tarea 1", false);
            var tarea2 = new Tarea("Tarea 2", "Descripcion de la tarea 2", true);
            await _repositorio.AgregarAsync(tarea1);
            await _repositorio.AgregarAsync(tarea2);

            // Act: Obtener todas las tareas desde la base de datos
            var tareas = await _repositorio.ObtenerTodosAsync();

            // Assert: Verificar que se obtuvieron correctamente
            Assert.NotNull(tareas);
            Assert.Equal(2, tareas.Count);
        }

        [Fact]
        public async Task ObtenerTareaPorId()
        {
            // Arrange: Crear y agregar una nueva tarea
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false);
            await _repositorio.AgregarAsync(tarea);

            // Act: Buscar la tarea por su ID
            var tareaObtenida = await _repositorio.BuscarPorIdAsync(tarea.Id);

            // Assert: Verificar que la tarea se obtuvo correctamente
            Assert.NotNull(tareaObtenida);
            Assert.Equal(tarea.MostrarTarea(), tareaObtenida.MostrarTarea());
        }
    }
}
