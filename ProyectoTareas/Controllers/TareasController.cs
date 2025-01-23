using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoTareas.Data;
using ProyectoTareas.Models;

namespace ProyectoTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly TareasDbContext _context;
        private readonly IMapper _mapper;

        public TareasController(TareasDbContext context, IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            try
            {
                var tareas = await _context.Tareas.ToListAsync();
                return Ok(tareas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener las tareas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDTO>> GetTarea(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("El ID proporcionado no es válido.");
                }

                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null)
                {
                    return NotFound("La tarea con el ID especificado no existe.");
                }

                var tareaDto = _mapper.Map<TareaDTO>(tarea);
                return Ok(tareaDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener la tarea: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<TareaDTO>> PostTarea(TareaDTO tareaDto)
        {
            try
            {
                if (tareaDto == null)
                {
                    return BadRequest("Los datos proporcionados son inválidos.");
                }

                var tarea = _mapper.Map<Tarea>(tareaDto);
                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetTarea", new { id = tarea.Id }, _mapper.Map<TareaDTO>(tarea));
                
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la tarea: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(Guid id, TareaDTO tareaDto)
        {
            try
            {
                var tareaExistente = await _context.Tareas.FindAsync(id);
                if (tareaExistente == null)
                {
                    return NotFound("La tarea con el ID especificado no existe.");
                }

                tareaExistente.Titulo = tareaDto.Titulo;
                tareaExistente.Descripcion = tareaDto.Descripcion;
                tareaExistente.Completada = tareaDto.Completada;

                _context.Entry(tareaExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return Ok(new { message = "La tarea fue actualizada correctamente." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(Guid id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea == null)
                {
                    return NotFound("La tarea no existe.");
                }

                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();

                return Ok(new { message = "La tarea fue eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarea: {ex.Message}");
            }
        }
    }
}

