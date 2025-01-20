namespace ProyectoTareas
{
    public interface IOpereaciones<T>
    {
        void Agregar(T item); //Añadir una tarea
        T BuscarPorId(Guid id); //Buscar una tarea por su ID
        IOpereaciones<T>ObtenerTodos(); //Obtener todas las tareas
        void Actualizar(T item); //Actualizar una tarea
        void Eliminar(Guid id); //Eliminar una tarea
    }
}
