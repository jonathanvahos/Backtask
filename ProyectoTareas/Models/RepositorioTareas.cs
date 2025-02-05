using Microsoft.EntityFrameworkCore;
using ProyectoTareas.Data;

namespace ProyectoTareas.Models
{
    // implementacion de la interfaz IOpereaciones para manejar las tareas
    public class RepositorioTareas : IOpereaciones<Tarea>
    {
        private readonly TareasDbContext _context;

        public RepositorioTareas(TareasDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public RepositorioTareas()
        {
        }

        public async Task AgregarAsync(Tarea item) 
        {
            _context.Tareas.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task<Tarea> BuscarPorIdAsync(Guid id)
        {
            return await _context.Tareas.FindAsync(id);
        }

        public async Task<List<Tarea>> ObtenerTodosAsync()
        {
            return await _context.Tareas.ToListAsync();
        }

        public async Task ActualizarAsync(Tarea item)
        {
            var tarea = await _context.Tareas.AsNoTracking().FirstOrDefaultAsync(t => t.Id == item.Id);
            if (tarea != null) 
            {
                //perservar la fecha de creacion
                item.FechaCreacion = tarea.FechaCreacion;

                //marcar la entidad como modificada, excluyendo la fecha de creacion
                _context.Entry(item).Property(t => t.FechaCreacion).IsModified = false;
                _context.Entry(item).State = EntityState.Modified;

                await _context.SaveChangesAsync();
            }
        }

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
