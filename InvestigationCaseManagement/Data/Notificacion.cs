using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InvestigationCaseManagement.Data
{
    public class Notificacion
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; }
        public IdentityUser Usuario { get; set; }

        [Required]
        public string Mensaje { get; set; }

        public bool Leida { get; set; } = false;

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public string NroExpediente()
        {
            var nroExpediente = Mensaje.Split(" ")[2];
            return nroExpediente;
        }

    }
}
