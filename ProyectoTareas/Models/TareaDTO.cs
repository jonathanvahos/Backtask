namespace ProyectoTareas.Models
{
    public class TareaDTO
    {
        //DTO para exportar solo los datos necesarios de una tarea
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Completada { get; set; }
    }
}
