using AutoMapper; // Importa la librería AutoMapper para realizar mapeos entre objetos.
using ProyectoTareas.Models; // Importa el espacio de nombres donde se encuentran las clases Tarea y TareaDTO.

namespace ProyectoTareas.Mappings
{
    // Clase que hereda de Profile, que es parte de AutoMapper.
    // Esta clase define los mapeos entre objetos.
    public class MappingProfile : Profile
    {
        // Constructor de la clase MappingProfile.
        public MappingProfile()
        {
            // Mapeo de la clase Tarea a la clase TareaDTO.
            // Esto permite convertir automáticamente un objeto de tipo Tarea en un objeto de tipo TareaDTO.
            CreateMap<Tarea, TareaDTO>();

            // Mapeo inverso de la clase TareaDTO a la clase Tarea.
            // Esto es útil en operaciones como POST o PUT, donde se recibe un TareaDTO y se necesita convertirlo en un Tarea.
            CreateMap<TareaDTO, Tarea>();
        }
    }
}