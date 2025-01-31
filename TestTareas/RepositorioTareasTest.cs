using ProyectoTareas.Models;
using System;
using Xunit;

namespace TestTareas
{
    public class RepositorioTareasTest
    {
        private readonly RepositorioTareas repositorio;

        public RepositorioTareasTest()
        {
            repositorio = new RepositorioTareas();
        }

        [Fact]// prueba unitaria para agregar una tarea

        public void AgregarTarea() 
        {
            // Arrange preparar el entorno y los datos de prueba
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1",true);

            // Act se ejecuta el metodo que se va a probar
            repositorio.Agregar(tarea);
            var tareaAgregada = repositorio.BuscarPorId(tarea.Id);

            // Assert se verifica que el resiltado sea el esperado
            Assert.NotNull(tareaAgregada); // verifica que la tarea no sea nula
            
            var resultadoEsperado = $"Tarea ID: {tarea.Id}\n" +
                        $"Titulo: {tarea.Titulo}\n" +
                        $"Descripcion: {tarea.Descripcion}\n" +
                        $"Completada: {tarea.Completada}\n" +
                        $"FechaCreacion: {tarea.FechaCreacion:yyyy-MM-dd}\n";


            Assert.Equal(resultadoEsperado, tareaAgregada.MostrarTarea()); // verifica que la tarea agregada sea la misma que la tarea buscada


        }

        [Fact]// prueba unitaria para buscar una tarea por su ID

        public void BuscarPorId() 
        {
            // Arrange preparar el entorno y los datos de prueba
            var tarea = new Tarea("Tarea 1", "Descripcion de la tarea 1",true);
            repositorio.Agregar(tarea);
            // Act se ejecuta el metodo que se va a probar
            var tareaBuscada = repositorio.BuscarPorId(tarea.Id);
            // Assert se verifica que el resiltado sea el esperado
            Assert.NotNull(tareaBuscada); // verifica que la tarea no sea nula
            Assert.Equal(tarea.Id, tareaBuscada.Id); // verifica el ID
        }

        [Fact] // prueba unitaria para el caso de que una tarea no exista

        public void BuscarPorIdNoExiste()
        {
            // Arrange crearm un ID que no exista en el Repositorio
            var idInexistente = Guid.NewGuid();

            // Act se intenta buscar una tarea con el ID inexistente
            var tareaBuscada = repositorio.BuscarPorId(idInexistente);

            // Assert se verifica que el resultado sea null
            Assert.Null(tareaBuscada);

        }

        [Fact] // prueba unitaria para Obtener todas las tareas del repositorio
        public void ObtenerTodos()
        {
            // Arrange preparar el entorno y los datos de prueba
            var tarea1 = new Tarea("Tarea 1", "Descripcion de la tarea 1", true);
            var tarea2 = new Tarea("Tarea 2", "Descripcion de la tarea 2", true);
            repositorio.Agregar(tarea1);
            repositorio.Agregar(tarea2);
            
            // Act se ejecuta el metodo que se va a probar
            var TodasLastareas = repositorio.ObtenerTodos();
            
            // Assert se verifica que la lista no sea nula y que contenga las tareas agregadas
            Assert.NotNull(TodasLastareas);
            
            Assert.Equal(2, TodasLastareas.Count);// verifica que la lista contenga 2 tareas
            Assert.Contains(tarea1, TodasLastareas); // verifica que la tarea1 este en la lista
            Assert.Contains(tarea2, TodasLastareas); // verifica que la tarea2 este en la lista
        }

        
        [Fact]// prueba unitaria para actualizar una tarea
        public void ActualizarTarea()
        {
            // Arrange preparar el entorno y los datos de prueba
            var tarea = new Tarea("Tarea especial", "Descripcion de la tarea especializada", true);
            repositorio.Agregar(tarea); // se agrega la tarea al repositorio
            tarea.Titulo = "Tarea actualizada"; // se actualiza el titulo
            tarea.Descripcion = "Descripcion actualizada"; // se actualiza la descripcion
            tarea.Completada = false; // se actualiza el estado de la tarea

            // Act se ejecuta el metodo
            repositorio.Actualizar(tarea);
            var tareaActualizada = repositorio.BuscarPorId(tarea.Id);// se crea una variable para buscar la tarea actualizada

            // Assert se verifica que la tarea se haya actualizado correctamente
            Assert.Equal("Tarea actualizada", tareaActualizada.Titulo); // verifica el titulo
            Assert.Equal("Descripcion actualizada", tareaActualizada.Descripcion); // verifica la descripcion
            Assert.False(tareaActualizada.Completada); // verifica si la tarea esta completada

        }

        [Fact]

        public void EliminarTarea()
        {
            // Arrange preparar el entorno y los datos de prueba
            var tarea = new Tarea("Tarea especial", "Descripcion de la tarea especializada", true);
            repositorio.Agregar(tarea); // se agrega la tarea al repositorio
            
            // Act se ejecuta el metodo
            repositorio.Eliminar(tarea.Id);
            var tareaEliminada = repositorio.BuscarPorId(tarea.Id);// se crea una variable para buscar la tarea eliminada
            
            // Assert se verifica que la tarea se haya eliminado correctamente
            Assert.Null(tareaEliminada); // verifica que la tarea no exista en el repositorio
        }

    }
}