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
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor httpContextAccessor)
       : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var auditorias = new List<Auditoria>();

        //    foreach (var entry in ChangeTracker.Entries())
        //    {
        //        if (entry.Entity is Caso || entry.Entity is Archivo)
        //        {
        //            var entidad = entry.Entity.GetType().Name;
        //            var datosJson = JsonSerializer.Serialize(entry.Entity); // Convertir entidad a JSON


        //            var entidadId = (int)entry.Property("Id").CurrentValue;

        //            var auditoria = new Auditoria
        //            {
        //                Entidad = entidad,
        //                EntidadId = entidadId,
        //                Usuario = UserContext,
        //                Accion = entry.State.ToString(),
        //                Detalle = entry.State == EntityState.Modified
        //                    ? string.Join(", ", entry.Properties
        //                        .Where(p => p.IsModified)
        //                        .Select(p => $"{p.Metadata.Name}: {p.OriginalValue} -> {p.CurrentValue}"))
        //                    : "Nuevo Registro",
        //                DatosEntidad = datosJson,
        //                Fecha = DateTime.Now
        //            };

        //            auditorias.Add(auditoria);
        //        }
        //    }

        //    await Auditorias.AddRangeAsync(auditorias);
        //    return await base.SaveChangesAsync(cancellationToken);
        //}

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var auditorias = new List<Auditoria>();
            var entidadesNuevas = new List<(object entidad, EntityEntry entry)>(); // Guardar las entidades nuevas

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Caso || entry.Entity is Archivo)
                {
                    var entidad = entry.Entity.GetType().Name;
                    var datosJson = JsonSerializer.Serialize(entry.Entity); // Convertir entidad a JSON

                    if (entry.State == EntityState.Added)
                    {
                        // No tenemos el Id todavía, lo guardamos para después
                        entidadesNuevas.Add((entry.Entity, entry));
                    }
                    else
                    {
                        var entidadId = (int)entry.Property("Id").CurrentValue;

                        var auditoria = new Auditoria
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
            if (auditorias.Any()) // Evitar inserción vacía
            {
                await Auditorias.AddRangeAsync(auditorias);
                await base.SaveChangesAsync(cancellationToken);
            }

            return resultado;
        }

        public DbSet<Caso> Casos { get; set; }
        public DbSet<Archivo> Archivos { get; set; }
        public DbSet<Auditoria> Auditorias { get; set; }
    }
}
