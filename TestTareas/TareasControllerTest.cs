using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProyectoTareas.Controllers;
using ProyectoTareas.Mappings;
using ProyectoTareas.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace TestTareas
{
    // Clase de prueba para TareasController
    public class TareasControllerTest
    {
        // Mock del repositorio
        private readonly Mock<IOpereaciones<Tarea>> _mockRepositorio;
        // Mapper de AutoMapper
        private readonly IMapper _mapper;
        // Controlador bajo prueba
        private readonly TareasController _tareasController;

        // Constructor de la clase de prueba
        public TareasControllerTest()
        {
            // Inicializa el mock del repositorio
            _mockRepositorio = new Mock<IOpereaciones<Tarea>>();
            // Configura AutoMapper con el perfil de mapeo
            var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
            // Crea el mapper
            _mapper = config.CreateMapper();
            // Inicializa el controlador con el mock y el mapper
            _tareasController = new TareasController(_mockRepositorio.Object, _mapper);
        }

        [Fact]
        // Test para obtener todas las tareas
        public async Task GetTareas()
        {
            // Arrange: Configura el mock para devolver una lista de tareas
            var tareas = new List<Tarea>
            {
                new Tarea("Tarea 1", "Descripcion de la tarea 1", false),
                new Tarea("Tarea 2", "Descripcion de la tarea 2", true),
                new Tarea("Tarea 3", "Descripcion de la tarea 3", false)
            };
            _mockRepositorio.Setup(repo => repo.ObtenerTodosAsync()).ReturnsAsync(tareas);

            // Act: Llama al método GetTareas del controlador
            var result = await _tareasController.GetTareas();
            // Verifica que el resultado sea OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            // Verifica que el valor sea IEnumerable<TareaDTO>
            var model = Assert.IsAssignableFrom<IEnumerable<TareaDTO>>(okResult.Value);
            // Verifica que la cantidad de tareas sea 3
            Assert.Equal(3, model.Count());
        }

        [Fact]
        // Test para obtener una tarea por id
        public async Task GetTareaId()
        {
            // Arrange: Configura el mock para devolver una tarea específica
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false);
            _mockRepositorio.Setup(repo => repo.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(tarea);

            // Act: Llama al método GetTarea del controlador
            var result = await _tareasController.GetTarea(Guid.NewGuid());
            // Verifica que el resultado sea OkObjectResult
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            // Verifica que el valor sea TareaDTO
            var model = Assert.IsType<TareaDTO>(okResult.Value);
            // Verifica que devuelva la tarea correcta
            Assert.Equal("Tarea 1", model.Titulo);
            Assert.Equal("Descripcion de la tarea 1", model.Descripcion);
            Assert.False(model.Completada);
        }

        [Fact]
        // Test para añadir una tarea
        public async Task PostTareas()
        {
            // Arrange: Configura el mock para añadir una tarea
            var tareaDto = new TareaDTO { Titulo = "Tarea 1", Descripcion = "Descripcion de la tarea 1", Completada = false };
            var tarea = _mapper.Map<Tarea>(tareaDto);
            _mockRepositorio.Setup(repo => repo.AgregarAsync(It.IsAny<Tarea>())).Returns(Task.CompletedTask);

            // Act: Llama al método PostTareas del controlador
            var result = await _tareasController.PostTareas(tareaDto);
            // Verifica que el resultado sea CreatedAtActionResult
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            // Verifica que el valor sea TareaDTO
            var returnValue = Assert.IsType<TareaDTO>(createdAtActionResult.Value);
            // Verifica que devuelva la tarea correcta
            Assert.Equal("Tarea 1", returnValue.Titulo);
            Assert.Equal("Descripcion de la tarea 1", returnValue.Descripcion);
            Assert.False(returnValue.Completada);
        }

        [Fact]
        // Test para actualizar una tarea
        public async Task PutTarea()
        {
            // Arrange: Configura el mock para buscar y actualizar una tarea
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false) { Id = Guid.NewGuid() };
            _mockRepositorio.Setup(repo => repo.BuscarPorIdAsync(tarea.Id)).ReturnsAsync(tarea);

            var tareaDtoUpdate = new TareaDTO
            {
                Id = tarea.Id,
                Titulo = "Tarea 1 Actualizada",
                Descripcion = "Descripcion de la tarea 1 Actualizada",
                Completada = true
            };

            // Act: Llama al método PutTarea del controlador
            var result = await _tareasController.PutTarea(tarea.Id, tareaDtoUpdate);
            // Verifica que el resultado sea OkObjectResult
            Assert.IsType<OkObjectResult>(result);

            // Verify: Verifica que el método ActualizarAsync se haya llamado con los parámetros correctos
            _mockRepositorio.Verify(repo => repo.ActualizarAsync(It.Is<Tarea>(t =>
                t.Id == tarea.Id &&
                t.Titulo == "Tarea 1 Actualizada" &&
                t.Descripcion == "Descripcion de la tarea 1 Actualizada" &&
                t.Completada)), Times.Once);
        }

        [Fact]
        // Test para eliminar una tarea
        public async Task DeleteTarea()
        {
            // Arrange: Configura el mock para buscar y eliminar una tarea
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false) { Id = Guid.NewGuid() };
            _mockRepositorio.Setup(repo => repo.BuscarPorIdAsync(tarea.Id)).ReturnsAsync(tarea);

            // Act: Llama al método DeleteTareas del controlador
            var result = await _tareasController.DeleteTareas(tarea.Id);
            // Verifica que el resultado sea OkObjectResult
            Assert.IsType<OkObjectResult>(result);

            // Verify: Verifica que el método EliminarAsync se haya llamado una vez
            _mockRepositorio.Verify(repo => repo.EliminarAsync(tarea.Id), Times.Once);
        }
    }
}

