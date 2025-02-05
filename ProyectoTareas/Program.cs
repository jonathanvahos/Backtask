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
            var builder = WebApplication.CreateBuilder(args);

            // Registrar AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            // Agregar EF Core al contenedor de servicios.
            builder.Services.AddDbContext<TareasDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            // Registrar RepositorioTareas
            builder.Services.AddScoped<IOpereaciones<Tarea>, RepositorioTareas>();

            // Registrar servicios de autorizacion
            builder.Services.AddAuthorization();

            // Registrar servicios de controladores
            builder.Services.AddControllers();


            // Agregar la configuraci�n de CORS
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

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Habilitar CORS
            app.UseCors("AllowLocalhost");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

