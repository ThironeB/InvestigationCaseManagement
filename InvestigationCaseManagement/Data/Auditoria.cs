namespace InvestigationCaseManagement.Data
{
    public class Auditoria
    {
        public int Id { get; set; }
        public string Entidad { get; set; } // Caso o Archivo
        public int EntidadId { get; set; } // Id del caso o archivo
        public string Usuario { get; set; } // Usuario que hizo el cambio
        public string Accion { get; set; } // Create, Update, Delete
        public string Detalle { get; set; } // Campos modificados
        public string DatosEntidad { get; set; } // JSON con toda la entidad
        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
