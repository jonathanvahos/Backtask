using Microsoft.EntityFrameworkCore;
using ProyectoTareas.Models;

namespace ProyectoTareas.Data
{
    // La clase TareasDbContext hereda de DbContext y se usa para interactuar con la base de datos
    public class TareasDbContext : DbContext
    {
        // Constructor que recibe las opciones de configuración de la base de datos y las pasa al constructor base
        public TareasDbContext(DbContextOptions<TareasDbContext> options)
            : base(options)
        {
        }

        // Define un DbSet de la entidad Tarea, que se corresponde con una tabla en la base de datos
        public DbSet<Tarea> Tareas { get; set; }

        // Configura el modelo de la base de datos, por ejemplo, reglas de validación y tipos de datos
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de la entidad Tarea
            modelBuilder.Entity<Tarea>(entity =>
            {
                // Configura la propiedad Titulo como obligatoria y con una longitud máxima de 200 caracteres
                entity.Property(t => t.Titulo).IsRequired().HasMaxLength(200);

                // Configura la propiedad Descripcion y con una longitud máxima de 500 caracteres
                entity.Property(t => t.Descripcion).HasMaxLength(500);

                // Configura la propiedad FechaCreacion para que tenga el valor por defecto de la fecha y hora actual cuando se cree la tarea
                entity.Property(t => t.FechaCreacion).HasDefaultValueSql("GETDATE()");
            });

            // Llama a la implementación base de OnModelCreating para realizar configuraciones adicionales
            base.OnModelCreating(modelBuilder);
        }
    }
}
