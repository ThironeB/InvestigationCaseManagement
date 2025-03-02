using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace InvestigationCaseManagement.Data
{
    public class Notificacion
    {
        public int Id { get; set; }

        [Required]
        public string UsuarioId { get; set; } // Usuario que recibe la notificación
        public IdentityUser Usuario { get; set; } // Relación con el usuario

        [Required]
        public string Mensaje { get; set; } // Mensaje de la notificación

        public bool Leida { get; set; } = false; // Indica si la notificación fue leída

        public DateTime FechaCreacion { get; set; } = DateTime.Now; // Fecha de creación de la notificación

        /// <summary>
        /// La funcion NroExpediente extrae y retorna la tercera palabra de una cadena dada.
        /// </summary>
        /// <returns>
        /// El método `NroExpediente` devuelve el tercer elemento del array obtenido al
        /// dividir la cadena `Mensaje` utilizando un espacio como delimitador.
        /// </returns>
        public string NroExpediente()
        {
            var nroExpediente = Mensaje.Split(" ")[2];
            return nroExpediente;
        }

    }
}
