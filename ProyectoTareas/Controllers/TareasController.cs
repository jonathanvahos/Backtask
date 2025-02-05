using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProyectoTareas.Models;

namespace ProyectoTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly IOpereaciones<Tarea> _repositorio;
        private readonly IMapper _mapper;

        public TareasController(IOpereaciones<Tarea> repositorio, IMapper mapper)
        {
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaDTO>>> GetTareas()
        {
            try
            {
                var tareas = await _repositorio.ObtenerTodosAsync();
                var tareasDto = _mapper.Map<IEnumerable<TareaDTO>>(tareas);
                return Ok(tareasDto);
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
                var tarea = await _repositorio.BuscarPorIdAsync(id);
                if (tarea == null)
                {
                    return NotFound("La tarea no existe.");
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
        public async Task<ActionResult<TareaDTO>> PostTareas(TareaDTO tareaDto)
        {
            try
            {
                if (tareaDto == null)
                {
                    return BadRequest("Los datos proporcionados son inválidos.");
                }

                var tarea = _mapper.Map<Tarea>(tareaDto);
                await _repositorio.AgregarAsync(tarea);
                return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, _mapper.Map<TareaDTO>(tarea));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar la tarea: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(Guid id, TareaDTO tareaDto)
        {
            try
            {
                if (id != tareaDto.Id)
                {
                    return BadRequest("Los IDs no coinciden.");
                }

                var tareaExistente = await _repositorio.BuscarPorIdAsync(id);
                if (tareaExistente == null)
                {
                    return NotFound("La tarea no existe.");
                }

                _mapper.Map(tareaDto, tareaExistente);
                await _repositorio.ActualizarAsync(tareaExistente);

                return Ok(new { message = "La tarea fue actualizada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTareas(Guid id)
        {
            try
            {
                var tarea = await _repositorio.BuscarPorIdAsync(id);
                if (tarea == null)
                {
                    return NotFound("La tarea no existe.");
                }

                await _repositorio.EliminarAsync(id);
                return Ok(new { message = "La tarea fue eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar la tarea: {ex.Message}");
            }
        }
    }
}

