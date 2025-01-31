namespace ProyectoTareas.Models
{
    public class TareaDTO
    {
        //DTO para exportar solo los datos necesarios de una tarea
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool Completada { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
