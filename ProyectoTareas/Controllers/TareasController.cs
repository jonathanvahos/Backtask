using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTareas.Data;  
using ProyectoTareas.Models; // se importa los modelos
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProyectoTareas.Controllers
{
    [Route("api/[controller]")]  // Define la ruta base del controlador.
    [ApiController]  // Indica que este controlador maneja solicitudes de API.
    public class TareasController : ControllerBase
    {
        private readonly TareasDbContext _context;  // Contexto de la base de datos.

        public TareasController(TareasDbContext context)
        {
            _context = context;  // Inicializa el contexto de la base de datos.
        }

        // Método para obtener todas las tareas.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            try
            {
                // Retorna la lista de todas las tareas desde la base de datos.
                return await _context.Tareas.ToListAsync();
            }
            catch (Exception ex)
            {
                // Captura cualquier error y devuelve un mensaje con el código 500.
                return StatusCode(500, $"Error al obtener las tareas: {ex.Message}");
            }
        }

        // Método para obtener una tarea específica por ID.
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTarea(Guid id)
        {
            try
            {
                // Busca la tarea en la base de datos por su ID.
                var tarea = await _context.Tareas.FindAsync(id);

                if (tarea == null)
                {
                    // Devuelve un código 404 si no se encuentra la tarea.
                    return NotFound();
                }

                return tarea;  // Retorna la tarea encontrada.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la tarea: {ex.Message}");
            }
        }

        // Método para actualizar una tarea completa por su ID.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(Guid id, Tarea tarea)
        {
            try
            {
                // Valida que el ID de la URL coincida con el ID del objeto enviado.
                if (id != tarea.Id)
                {
                    return BadRequest();
                }

                // Marca la tarea como modificada en el contexto.
                _context.Entry(tarea).State = EntityState.Modified;

                try
                {
                    // Intenta guardar los cambios en la base de datos.
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Maneja errores de concurrencia (cuando la tarea no existe).
                    if (!TareaExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();  // Retorna un código 204 sin contenido.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        // Método para crear una nueva tarea.
        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
        {
            try
            {
                // Agrega la nueva tarea al contexto.
                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();  // Guarda los cambios en la base de datos.

                // Retorna un código 201 indicando que la tarea se creó con éxito.
                return CreatedAtAction("GetTarea", new { id = tarea.Id }, tarea);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la tarea: {ex.Message}");
            }
        }

        // Método para eliminar una tarea por su ID.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(Guid id)
        {
            try
            {
                // Busca la tarea por su ID.
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null)
                {
                    // Retorna un código 404 si no se encuentra la tarea.
                    return NotFound();
                }

                // Elimina la tarea del contexto y guarda los cambios.
                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();

                return NoContent();  // Retorna un código 204 sin contenido.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarea: {ex.Message}");
            }
        }

        


        // Método auxiliar para verificar si una tarea existe.
        private bool TareaExists(Guid id)
        {
            try
            {
                // Retorna true si existe una tarea con el ID especificado.
                return _context.Tareas.Any(e => e.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al verificar si la tarea existe: {ex.Message}");
            }
        }
    }
}

