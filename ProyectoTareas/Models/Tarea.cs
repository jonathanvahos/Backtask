namespace ProyectoTareas.Models
{
    // clase que hereda de la clase TareaBase
    public class Tarea : TareaBase
    {
        // Atributos de la clase Privados
        private string titulo;
        private string descripcion;
        private Boolean completada;

        //Propiedades Publicas

        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public Boolean Completada
        {
            get { return completada; }
            set { completada = value; }
        }

        // Constructor vacio rrequerido por Entity Framework

        public Tarea() : base()
        {
        }

        public Tarea(string titulo, string descripcion, Boolean completada) : base()
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Completada = completada;
        }

        // Metodo que muestra la tarea

        public override string MostrarTarea()
        {
            return $"Tarea ID: {Id}\n" +
           $"Titulo: {Titulo}\n" +
           $"Descripcion: {Descripcion}\n" +
           $"Completada: {Completada}\n" +
           $"FechaCreacion: {FechaCreacion:yyyy-MM-dd}\n";
        }
    }
}
