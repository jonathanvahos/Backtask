using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProyectoTareas.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProyectoTareas.Controllers
{
    // Define la ruta base para el controlador y especifica que es un controlador de API
    [Route("api/[controller]")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        // Dependencias inyectadas
        private readonly IOpereaciones<Tarea> _repositorio; // Repositorio para operaciones CRUD
        private readonly IMapper _mapper; // AutoMapper para convertir entre Tarea y TareaDTO

        // Constructor que inicializa las dependencias inyectadas
        public TareasController(IOpereaciones<Tarea> repositorio, IMapper mapper)
        {
            // Verifica que los parámetros no sean nulos, lanza una excepción si lo son
            _repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        // Método GET para obtener todas las tareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TareaDTO>>> GetTareas()
        {
            try
            {
                // Llama al repositorio para obtener todas las tareas
                var tareas = await _repositorio.ObtenerTodosAsync();
                // Usa AutoMapper para convertir la lista de Tarea a TareaDTO
                var tareasDto = _mapper.Map<IEnumerable<TareaDTO>>(tareas);
                // Devuelve la lista de TareaDTO con un código de estado 200 (OK)
                return Ok(tareasDto);
            }
            catch (Exception ex)
            {
                // Manejo de errores, devuelve un código de estado 500 (Internal Server Error) con el mensaje de error
                return StatusCode(500, $"Error al obtener las tareas: {ex.Message}");
            }
        }

        // Método GET para obtener una tarea por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TareaDTO>> GetTarea(Guid id)
        {
            try
            {
                // Llama al repositorio para buscar la tarea por ID
                var tarea = await _repositorio.BuscarPorIdAsync(id);
                // Verifica si la tarea existe
                if (tarea == null)
                {
                    // Devuelve un código de estado 404 (Not Found) si la tarea no existe
                    return NotFound("La tarea no existe.");
                }
                // Usa AutoMapper para convertir la tarea a TareaDTO
                var tareaDto = _mapper.Map<TareaDTO>(tarea);
                // Devuelve la tarea encontrada con un código de estado 200 (OK)
                return Ok(tareaDto);
            }
            catch (Exception ex)
            {
                // Manejo de errores, devuelve un código de estado 500 (Internal Server Error) con el mensaje de error
                return StatusCode(500, $"Error al obtener la tarea: {ex.Message}");
            }
        }

        // Método POST para agregar una nueva tarea
        [HttpPost]
        public async Task<ActionResult<TareaDTO>> PostTareas(TareaDTO tareaDto)
        {
            try
            {
                // Verifica que los datos proporcionados no sean nulos
                if (tareaDto == null)
                {
                    // Devuelve un código de estado 400 (Bad Request) si los datos son inválidos
                    return BadRequest("Los datos proporcionados son inválidos.");
                }

                // Usa AutoMapper para convertir TareaDTO a Tarea
                var tarea = _mapper.Map<Tarea>(tareaDto);
                // Llama al repositorio para agregar la nueva tarea
                await _repositorio.AgregarAsync(tarea);
                // Devuelve la tarea creada con un código de estado 201 (Created)
                return CreatedAtAction(nameof(GetTarea), new { id = tarea.Id }, _mapper.Map<TareaDTO>(tarea));
            }
            catch (Exception ex)
            {
                // Manejo de errores, devuelve un código de estado 500 (Internal Server Error) con el mensaje de error
                return StatusCode(500, $"Error al agregar la tarea: {ex.Message}");
            }
        }

        // Método PUT para actualizar una tarea existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(Guid id, TareaDTO tareaDto)
        {
            try
            {
                // Verifica que los IDs coincidan
                if (id != tareaDto.Id)
                {
                    // Devuelve un código de estado 400 (Bad Request) si los IDs no coinciden
                    return BadRequest("Los IDs no coinciden.");
                }

                // Llama al repositorio para buscar la tarea existente por ID
                var tareaExistente = await _repositorio.BuscarPorIdAsync(id);
                // Verifica si la tarea existe
                if (tareaExistente == null)
                {
                    // Devuelve un código de estado 404 (Not Found) si la tarea no existe
                    return NotFound("La tarea no existe.");
                }

                // Usa AutoMapper para actualizar la tarea existente con los datos de TareaDTO
                _mapper.Map(tareaDto, tareaExistente);
                // Llama al repositorio para actualizar la tarea
                await _repositorio.ActualizarAsync(tareaExistente);

                // Devuelve un código de estado 200 (OK) con un mensaje de éxito
                return Ok(new { message = "La tarea fue actualizada correctamente." });
            }
            catch (Exception ex)
            {
                // Manejo de errores, devuelve un código de estado 500 (Internal Server Error) con el mensaje de error
                return StatusCode(500, $"Error al actualizar la tarea: {ex.Message}");
            }
        }

        // Método DELETE para eliminar una tarea por ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTareas(Guid id)
        {
            try
            {
                // Llama al repositorio para buscar la tarea por ID
                var tarea = await _repositorio.BuscarPorIdAsync(id);
                // Verifica si la tarea existe
                if (tarea == null)
                {
                    // Devuelve un código de estado 404 (Not Found) si la tarea no existe
                    return NotFound("La tarea no existe.");
                }

                // Llama al repositorio para eliminar la tarea
                await _repositorio.EliminarAsync(id);
                // Devuelve un código de estado 200 (OK) con un mensaje de éxito
                return Ok(new { message = "La tarea fue eliminada correctamente." });
            }
            catch (Exception ex)
            {
                // Manejo de errores, devuelve un código de estado 500 (Internal Server Error) con el mensaje de error
                return StatusCode(500, $"Error al eliminar la tarea: {ex.Message}");
            }
        }
    }
}

