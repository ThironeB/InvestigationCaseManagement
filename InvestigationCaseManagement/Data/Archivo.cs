using Microsoft.AspNetCore.Identity;

namespace InvestigationCaseManagement.Data
{
    public class Archivo
    {
        public int Id { get; set; } // Identificador único del archivo

        public string? Cedula { get; set; } // Cédula del cliente

        public string? Nombre { get; set; } // Nombre del cliente

        public string? Apellido { get; set; } // Apellido del cliente

        public int? EmpresaId { get; set; } // FK para la empresa
        public Empresa Empresa => Empresa.ObtenerEmpresa().FirstOrDefault(t => t.Id == EmpresaId) ?? new Empresa();

        public string? Serial { get; set; } // Serial del equipo

        public string? TipoEquipo { get; set; } // Tipo de equipo

        public string? Marca { get; set; } // Marca del equipo

        public string? Modelo { get; set; } // Modelo del equipo

        public string? Observaciones {  get; set; } // Observaciones del equipo

        public string Estado {  get; set; } // Estado del equipo

        public string? InvestigadorId { get; set; } // FK para el investigador asignado

        public IdentityUser Investigador { get; set; } // Relación con el investigador

        public DateTime? FechaCreacion { get; set; } // Fecha de creación del archivo

        public DateTime? FechaCierre {  get; set; } // Fecha de cierre del archivo

    }
}
