using Microsoft.EntityFrameworkCore;
using ProyectoTareas.Data;
using ProyectoTareas.Mappings;
using ProyectoTareas.Models;

namespace ProyectoTareas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Crear el builder recibiendo los argumentos (esto es importante para que se pueda configurar el entorno)
            var builder = WebApplication.CreateBuilder(args);

            // Registrar AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Registrar el DbContext de forma condicional según el entorno
            if (builder.Environment.IsEnvironment("Testing"))
            {
                // En entorno de pruebas, usar base de datos en memoria
                builder.Services.AddDbContext<TareasDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            }
            else
            {
                // En otros entornos (Desarrollo, Producción), usar SQL Server
                builder.Services.AddDbContext<TareasDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
            }

            // Registrar RepositorioTareas
            builder.Services.AddScoped<IOpereaciones<Tarea>, RepositorioTareas>();

            // Registrar servicios de autorización
            builder.Services.AddAuthorization();

            // Registrar servicios de controladores
            builder.Services.AddControllers();

            // Agregar la configuración de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")  // URL del front-end Angular
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configurar la tubería HTTP
            if (app.Environment.IsDevelopment() || app.Environment.IsEnvironment("Testing"))
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Deshabilitar la redirección HTTPS en entorno de pruebas
            if (!app.Environment.IsEnvironment("Testing"))
            {
                app.UseHttpsRedirection();
            }

            app.UseCors("AllowLocalhost");
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}


