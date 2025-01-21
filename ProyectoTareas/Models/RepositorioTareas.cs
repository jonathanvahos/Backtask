namespace ProyectoTareas.Models
{
    // implementacion de la interfaz IOpereaciones para manejar las tareas
    public class RepositorioTareas : IOpereaciones<Tarea>
    {
        //lista interna para almacenar las tareas
        private readonly List<Tarea> tareas = new List<Tarea>();

        //metodo para añadir una tarea
        public void Agregar(Tarea item)
        {
            tareas.Add(item);
        }

        //metodo para buscar una tarea por su ID
        public Tarea BuscarPorId(Guid id)
        {
            return tareas.FirstOrDefault(t => t.Id == id);
        }

        //metodo para obtener todas las tareas
        public List<Tarea> ObtenerTodos()
        {
            return tareas;
        }

        //metodo para actualizar una tarea
        public void Actualizar(Tarea item)
        {
            var tarea = BuscarPorId(item.Id);
            if (tarea != null)
            {
                tarea.Titulo = item.Titulo;
                tarea.Descripcion = item.Descripcion;
                tarea.Completada = item.Completada;
            }
        }

        //metodo para eliminar una tarea
        public void Eliminar(Guid id)
        {
            var tarea = BuscarPorId(id);
            if (tarea != null)
            {
                tareas.Remove(tarea);
            }
        }

    }
}
