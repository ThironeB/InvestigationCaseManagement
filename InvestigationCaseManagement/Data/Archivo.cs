using Microsoft.AspNetCore.Identity;

namespace InvestigationCaseManagement.Data
{
    public class Archivo
    {
        public int Id { get; set; }

        public string? Cedula { get; set; }

        public string? Nombre { get; set; }

        public string? Apellido { get; set; }

        public int? EmpresaId { get; set; }
        public Empresa Empresa => Empresa.ObtenerEmpresa().FirstOrDefault(t => t.Id == EmpresaId) ?? new Empresa();

        public string? Serial { get; set; }

        public string? TipoEquipo { get; set; }

        public string? Marca { get; set; }

        public string? Modelo { get; set; }

        public string? Observaciones {  get; set; }

        public string Estado {  get; set; }

        public string? InvestigadorId { get; set; } // FK para el investigador asignado

        public IdentityUser Investigador { get; set; } // Relación con el investigador

        public DateTime? FechaCreacion { get; set; }

        public DateTime? FechaCierre {  get; set; }

    }
}
