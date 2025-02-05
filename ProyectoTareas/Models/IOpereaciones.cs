
namespace ProyectoTareas.Models
{
    public interface IOpereaciones<T>
    {
        Task AgregarAsync(T item); // añadir una tarea
        Task<T> BuscarPorIdAsync(Guid id); // buscar una tarea por id
        Task<List<T>> ObtenerTodosAsync(); // obtener todas las tareas
        Task ActualizarAsync(T item); // actualizar una tarea
        Task EliminarAsync(Guid id); // eliminar una tarea

    }
}
