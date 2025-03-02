using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvestigationCaseManagement.Data;
using InvestigationCaseManagement.Data.Utilities;
using Microsoft.EntityFrameworkCore;

namespace InvestigationCaseManagement.Services
{
    public class NotificationService
    {
        private readonly ApplicationDbContext _context;

        /* Contructor para la clase NotificationService */
        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// La función CheckAndSendNotificationsAsync busca casos que requieran atención y envía
        ///  notificaciones a los administradores e investigadores si es necesario.
        /// </summary>
        public async Task CheckAndSendNotificationsAsync()
        {
            var casos = await _context.Casos
                .Where(c => c.Estado != EstadoCaso.Cerrado.ToString())
                .ToListAsync();

            foreach (var caso in casos)
            {
                if (caso.NecesitaAtencion()) //Casos que necesitan atencion. Tienen +1 dias sin atencion.
                {
                    var administradores = await _context.Users
                        .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == RoleIdentifier.Administrador.ToString()))
                        .ToListAsync();

                    // Enviar notificaciones a los administradores
                    foreach (var admin in administradores)
                    {
                        await CrearNotificacionAsync(admin.Id, $"El caso {caso.NumeroExpediente} necesita atención.");
                    }

                    // Enviar notificaciones a los investigadores
                    if (!string.IsNullOrEmpty(caso.InvestigadorId))
                    {
                        await CrearNotificacionAsync(caso.InvestigadorId, $"El caso {caso.NumeroExpediente} necesita atención.");
                    }
                }
            }
        }

        /// <summary>
        /// Crea una nueva notificación en la base de datos para un usuario específico con un mensaje determinado.
        /// </summary>
        /// <param name="usuarioId">El parametro `usuarioId` se utiliza para especificar el ID del usuario
        /// para el cual queremos recuperar las notificaciones no leídas.</param>
        /// <param name="mensaje">El parametro `mensaje` representa el mensaje asociado a la notificacion
        /// que se creara.</param>
        public async Task CrearNotificacionAsync(string usuarioId, string mensaje)
        {
            var notificacion = new Notificacion
            {
                UsuarioId = usuarioId,
                Mensaje = mensaje
            };

            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();
        }

        
        /// <summary>
        /// Recupera una lista de notificaciones no leídas
        /// para un usuario específico de forma asincrónica.
        /// </summary>
        /// <param name="usuarioId">El parametro `usuarioId` se utiliza para especificar el ID del usuario
        /// para el cual queremos recuperar las notificaciones no leídas.</param>
        /// <returns>
        /// Una lista de notificaciones no leídas para un usuario específico, ordenadas por fecha de creación en orden descendente.
        /// </returns>
        public async Task<List<Notificacion>> ObtenerNotificacionesNoLeidasAsync(string usuarioId)
        {
            return await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .OrderByDescending(n => n.FechaCreacion)
                .ToListAsync();
        }

        /// <summary>
        /// La función `MarcarComoLeidaAsync` marca una notificación como leída
        /// de forma asincrónica.
        /// </summary>
        /// <param name="notificacionId">El parametro `notificacionId` es el identificador único de la
        /// notificación que desea marcar como leída.</param>
        public async Task MarcarComoLeidaAsync(int notificacionId)
        {
            var notificacion = await _context.Notificaciones.FindAsync(notificacionId);
            if (notificacion != null)
            {
                notificacion.Leida = true;
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// La función `MarcarComoNoLeidaAsync` marca una notificación como no leída
        /// de forma asincrónica.
        /// </summary>
        /// <param name="usuarioId">El parametro `usuarioId` es el identificador único del usuario
        /// para el cual desea marcar todas las notificaciones como no leídas.</param>
        /// <returns>
        /// El método `CountObtenerNotificacionesNoLeidas` devuelve un valor entero, 
        /// ya sea 1 o 0, en función de si hay notificaciones no leídas para el `usuarioId` 
        /// especificado en la colección `_context.Notificaciones`.
        /// </returns>
        public int CountObtenerNotificacionesNoLeidas(string usuarioId)
        {
            return _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .Any() ? 1 : 0;
        }

        /// <summary>
        /// La función `ObtenerCasoPorNroExpedienteAsync` recupera de forma asincrónica un caso 
        /// por su número de expediente.
        /// </summary>
        /// <param name="nroExpediente">El parametro `nroExpediente` es una cadena que representa el
        /// número del expediente que desea recuperar de forma asincrónica de la base de datos.
        /// <returns>
        /// Un objeto `Caso` que representa un caso en la base de datos que coincide con el número de expediente
        /// proporcionado, o `null` si no se encuentra ningún caso con ese número de expediente.
        /// </returns>
        public async Task<Caso> ObtenerCasoPorNroExpedienteAsync(string nroExpediente)
        {
            return await _context.Casos
                .FirstOrDefaultAsync(c => c.NumeroExpediente == nroExpediente);
        }
    }
}
