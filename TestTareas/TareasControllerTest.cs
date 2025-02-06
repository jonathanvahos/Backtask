using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProyectoTareas.Controllers;
using ProyectoTareas.Mappings;
using ProyectoTareas.Models;

namespace TestTareas;

public class TareasControllerTest
{
    private readonly Mock<IOpereaciones<Tarea>> _mockRepositorio;
    private readonly IMapper _mapper;
    private readonly TareasController _tareasController;

    public TareasControllerTest()
    {
        _mockRepositorio = new Mock<IOpereaciones<Tarea>>();
        var config = new MapperConfiguration(cfg => cfg.AddProfile(new MappingProfile()));
        _mapper = config.CreateMapper();
        _tareasController = new TareasController(_mockRepositorio.Object, _mapper);
    }

    [Fact]
    // Test para obtener todas las tareas
    public async Task GetTareas()
    {
        var tareas = new List<Tarea>
        {
            new Tarea("Tarea 1", "Descripcion de la tarea 1", false),
            new Tarea("Tarea 2", "Descripcion de la tarea 2", true),
            new Tarea("Tarea 3", "Descripcion de la tarea 3", false)
        };
        _mockRepositorio.Setup(repo => repo.ObtenerTodosAsync()).ReturnsAsync(tareas);

        var result = await _tareasController.GetTareas();
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsAssignableFrom<IEnumerable<TareaDTO>>(okResult.Value);
        Assert.Equal(3, model.Count());
    }

    [Fact]
    // Test para obtener una tarea por id
    public async Task GetTareaId()
    {
        var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false);
        _mockRepositorio.Setup(repo => repo.BuscarPorIdAsync(It.IsAny<Guid>())).ReturnsAsync(tarea);

        var result = await _tareasController.GetTarea(Guid.NewGuid());
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var model = Assert.IsType<TareaDTO>(okResult.Value);
        Assert.Equal("Tarea 1", model.Titulo);
    }
    // Test para añadir una tarea
    [Fact]
    public async Task PostTareas()
    {
        var tareaDto = new TareaDTO { Titulo = "Tarea 1", Descripcion = "Descripcion de la tarea 1", Completada = false };
        var tarea = _mapper.Map<Tarea>(tareaDto);
        _mockRepositorio.Setup(repo => repo.AgregarAsync(It.IsAny<Tarea>())).Returns(Task.CompletedTask);

        var result = await _tareasController.PostTareas(tareaDto);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnValue = Assert.IsType<TareaDTO>(createdAtActionResult.Value);
        Assert.Equal("Tarea 1", returnValue.Titulo);
    }

    [Fact]
    // Test para actualizar una tarea
    public async Task PutTarea()
    {
        var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false) { Id = Guid.NewGuid() };
        _mockRepositorio.Setup(repo => repo.BuscarPorIdAsync(tarea.Id)).ReturnsAsync(tarea);

        var tareaDtoUpdate = new TareaDTO
        {
            Id = tarea.Id,
            Titulo = "Tarea 1 Actualizada",
            Descripcion = "Descripcion de la tarea 1 Actualizada",
            Completada = true
        };

        var result = await _tareasController.PutTarea(tarea.Id, tareaDtoUpdate);
        Assert.IsType<OkObjectResult>(result);

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
        var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1", false) { Id = Guid.NewGuid() };
        _mockRepositorio.Setup(repo => repo.BuscarPorIdAsync(tarea.Id)).ReturnsAsync(tarea);

        var result = await _tareasController.DeleteTareas(tarea.Id);
        Assert.IsType<OkObjectResult>(result);

        _mockRepositorio.Verify(repo => repo.EliminarAsync(tarea.Id), Times.Once);
    }
}

