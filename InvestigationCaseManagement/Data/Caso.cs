using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace InvestigationCaseManagement.Data
{
    public class Caso
    {
        public int Id { get; set; }

        [Required]
        public string NumeroExpediente { get; set; }

        public string InvestigadorId { get; set; } // FK para el investigador asignado
        public IdentityUser Investigador { get; set; } // Relación con el investigador

        [Required]
        public DateTime FechaInicio { get; set; } = DateTime.Now.Date; // Fecha actual por defecto

        [Required]
        public int DuracionDias { get; set; } // Duración en días

        [Required]
        public string TipoCaso { get; set; } // Tipo de caso (Gestión, Reclamo, Caso)

        [Required]
        public string TipoIrregularidad { get; set; } // Tipo de irregularidad

        [Required]
        public string SubtipoIrregularidad { get; set; } // Subtipo de irregularidad

        [Required]
        public string MovilAfectado { get; set; } // Móvil afectado

        [Required]
        public string ObjetivoAgraviado { get; set; } // Objetivo/Agraviado

        public string Incidencia { get; set; } // Incidencia
        public string ModusOperandi { get; set; } // Modus Operandi
        public string AreaApoyo { get; set; } // Área de apoyo
        public string OrigenCaso { get; set; } // Origen del caso
        public string DetallesFraude { get; set; } // Detalles de fraude
        public string Actuaciones { get; set; } // Actuaciones realizadas
        public string Conclusiones { get; set; } // Conclusiones
        public string Recomendaciones { get; set; } // Recomendaciones
        public string Observaciones { get; set; } // Observaciones
        public string Soporte { get; set; } // Soporte
        public string Estado { get; set; } // Estado del caso (Abierto, Asignado, Cerrado, Re-abierto)
    }
}
