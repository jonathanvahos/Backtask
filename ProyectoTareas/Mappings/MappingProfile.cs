using AutoMapper;
using ProyectoTareas.Models;

namespace ProyectoTareas.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeo de Tarea a TareaDTO
            CreateMap<Tarea, TareaDTO>();

            // Mapeo inverso de TareaDTO a Tarea (útil en operaciones como POST o PUT)
            CreateMap<TareaDTO, Tarea>();
        }


    }
}