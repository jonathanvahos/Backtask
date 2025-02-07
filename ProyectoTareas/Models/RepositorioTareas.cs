

using Microsoft.EntityFrameworkCore;
using ProyectoTareas.Data;

namespace ProyectoTareas.Models
{
    // Implementación de la interfaz IOpereaciones para manejar las tareas
    public class RepositorioTareas : IOpereaciones<Tarea>
    {
        // Contexto de la base de datos
        private readonly TareasDbContext _context;

        // Constructor que recibe el contexto de la base de datos
        public RepositorioTareas(TareasDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));// Validar que el contexto no sea nulo
        }

        // Método para agregar una nueva tarea a la base de datos
        public async Task AgregarAsync(Tarea item)
        {
            _context.Tareas.Add(item);
            await _context.SaveChangesAsync();
        }

        // Método para buscar una tarea por su ID11
        public async Task<Tarea> BuscarPorIdAsync(Guid id)
        {
            return await _context.Tareas.FindAsync(id);
        }

        // Método para obtener todas las tareas de la base de datos
        public async Task<List<Tarea>> ObtenerTodosAsync()
        {
            return await _context.Tareas.ToListAsync();
        }

        // Método para actualizar una tarea existente en la base de datos
        public async Task ActualizarAsync(Tarea item)
        {
            var tarea = await _context.Tareas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == item.Id);
            if (tarea != null)
            {
                // Preservar la fecha de creación
                item.FechaCreacion = tarea.FechaCreacion;

                // Marcar la entidad como modificada, excluyendo la fecha de creación
                _context.Entry(item).Property(t => t.FechaCreacion).IsModified = false;
                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }

        // Método para eliminar una tarea por su ID
        public async Task EliminarAsync(Guid id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea != null)
            {
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();
            }
        }
    }
}
