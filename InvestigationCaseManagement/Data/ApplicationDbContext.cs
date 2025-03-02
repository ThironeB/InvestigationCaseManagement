using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace InvestigationCaseManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userContext;

        /* La propiedad `UserContext` es una propiedad de acceso que se
        encarga de recuperar la información del contexto de usuario. */
        public string UserContext { get {
                if (!string.IsNullOrEmpty(_userContext))
                {
                    return _userContext;
                } 
                else
                {
                    _userContext = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "Sistema";
                    return _userContext ;
                }
            } 
        }

        /* Constructor para la clase ApplicationDbContext */
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
       : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        /// <summary>
        /// La funcion `SaveChangesAsync` en C# guarda de forma asincrona los cambios en las entidades,
        //  mientras también registra información de auditoría para ciertos tipos de entidades.
        /// </summary>
        /// <param name="CancellationToken">El parámetro `CancellationToken` en el método `SaveChangesAsync`
        /// le permite pasar un token que se puede utilizar para solicitar la cancelación de una
        /// operación asincrónica. Este token se puede utilizar para propagar la notificación de que las
        /// operaciones deben cancelarse.</param>
        /// <returns>
        /// El método `SaveChangesAsync` devuelve un valor entero que representa el resultado de guardar
        /// los cambios en la base de datos. Este valor se obtiene al llamar a `base.SaveChangesAsync(cancellationToken)`
        /// y se almacena en la variable `resultado` antes de realizar cualquier procesamiento adicional.
        /// Este valor entero representa típicamente el número de entidades afectadas por la operación de guardado.
        /// </returns>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditorias = new List<Auditoria>();
            var entidadesNuevas = new List<(object entidad, EntityEntry entry)>(); // Guardar las entidades nuevas

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Caso || entry.Entity is Archivo) // Solo auditar Casos y Archivos
                {
                    var entidad = entry.Entity.GetType().Name; // Nombre de la entidad
                    var datosJson = JsonSerializer.Serialize(entry.Entity); // Convertir entidad a JSON

                    if (entry.State == EntityState.Added) // Si es una entidad nueva
                    {
                        // No tenemos el Id todavía, lo guardamos para después
                        entidadesNuevas.Add((entry.Entity, entry));
                    }
                    else
                    {
                        var entidadId = (int)entry.Property("Id").CurrentValue;

                        var auditoria = new Auditoria // Crear objeto Auditoria
                        {
                            Entidad = entidad,
                            EntidadId = entidadId,
                            Usuario = UserContext,
                            Accion = entry.State.ToString(),
                            Detalle = entry.State == EntityState.Modified
                                ? string.Join(", ", entry.Properties
                                    .Where(p => p.IsModified)
                                    .Select(p => $"{p.Metadata.Name}: {p.OriginalValue} -> {p.CurrentValue}"))
                                : "Nuevo Registro",
                            DatosEntidad = datosJson,
                            Fecha = DateTime.Now
                        };

                        // Agregar a la lista de auditorías
                        auditorias.Add(auditoria);
                    }
                }
            }

            // Guardar cambios en la BD para obtener los Ids generados
            var resultado = await base.SaveChangesAsync(cancellationToken);

            // Ahora que las entidades tienen Id, registramos su auditoría
            foreach (var (entidad, entry) in entidadesNuevas)
            {
                var entidadId = (int)entry.Property("Id").CurrentValue;

                auditorias.Add(new Auditoria
                {
                    Entidad = entidad.GetType().Name,
                    EntidadId = entidadId, // Ahora tiene el Id correcto
                    Usuario = UserContext,
                    Accion = "Added",
                    Detalle = "Nuevo Registro",
                    DatosEntidad = JsonSerializer.Serialize(entidad),
                    Fecha = DateTime.Now
                });
            }

            // Guardamos las auditorías con los Ids correctos
            if (auditorias.Any() && UserContext != "Sistema") // Evitar inserción vacía o inserciones por el sistema de notificaciones
            {
                await Auditorias.AddRangeAsync(auditorias);
                await base.SaveChangesAsync(cancellationToken);
            }

            return resultado;
        }

        /* Las siguientes propiedades representan tablas de base de datos utilizando Entity Framework Core. */
        public DbSet<Caso> Casos { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
        public DbSet<Notificacion> Notificaciones { get; set; }

    }
}
