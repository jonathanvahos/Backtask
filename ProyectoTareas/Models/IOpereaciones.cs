
namespace ProyectoTareas.Models
{
    public interface IOpereaciones<T>
    {
        void Agregar(T item); //Añadir una tarea
        T BuscarPorId(Guid id); //Buscar una tarea por su ID
        List<T> ObtenerTodos(); //Obtener todas las tareas
        void Actualizar(T item); //Actualizar una tarea
        void Eliminar(Guid id); //Eliminar una tarea
        
    }
}
