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

        public NotificationService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CheckAndSendNotificationsAsync()
        {
            var casos = await _context.Casos
                .Where(c => c.Estado != EstadoCaso.Cerrado.ToString())
                .ToListAsync();

            foreach (var caso in casos)
            {
                if (caso.NecesitaAtencion())
                {
                    var administradores = await _context.Users
                        .Where(u => _context.UserRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == "91790a12-a288-4bf4-a56a-ca29740d28a5"))
                        .ToListAsync();

                    foreach (var admin in administradores)
                    {
                        await CrearNotificacionAsync(admin.Id, $"El caso {caso.NumeroExpediente} necesita atención.");
                    }

                    if (!string.IsNullOrEmpty(caso.InvestigadorId))
                    {
                        await CrearNotificacionAsync(caso.InvestigadorId, $"El caso {caso.NumeroExpediente} necesita atención.");
                    }
                }
            }
        }

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

        public async Task<List<Notificacion>> ObtenerNotificacionesNoLeidasAsync(string usuarioId)
        {
            return await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .OrderByDescending(n => n.FechaCreacion)
                .ToListAsync();
        }

        public async Task MarcarComoLeidaAsync(int notificacionId)
        {
            var notificacion = await _context.Notificaciones.FindAsync(notificacionId);
            if (notificacion != null)
            {
                notificacion.Leida = true;
                await _context.SaveChangesAsync();
            }
        }

        public int CountObtenerNotificacionesNoLeidas(string usuarioId)
        {
            return _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId && !n.Leida)
                .Any() ? 1 : 0;
        }

        public async Task<Caso> ObtenerCasoPorNroExpedienteAsync(string nroExpediente)
        {
            return await _context.Casos
                .FirstOrDefaultAsync(c => c.NumeroExpediente == nroExpediente);
        }
    }
}
