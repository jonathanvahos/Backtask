namespace ProyectoTareas.Models
{
    // clase abstracta que representa una tarea base    
    public abstract class TareaBase
    {
        // Atributos de la clase Privados
        private Guid id;
        private DateTime fechaCreacion;

        //Propiedades Publicas

        public Guid Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        // Constructor de la clase
        public TareaBase()
        {
            Id = Guid.NewGuid();
            FechaCreacion = DateTime.Now;
        }


        // metodo abstracto que se implementara en las clases hijas
        public abstract String MostrarTarea();


    }
}
