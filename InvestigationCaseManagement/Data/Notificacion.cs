using Microsoft.AspNetCore.Identity;
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

    }
}
