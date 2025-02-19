namespace InvestigationCaseManagement.Data
{
    public class ProcesoCorregido
    {
        public int Id { get; set; }
        public string Proceso { get; set; } = string.Empty;

        public static List<ProcesoCorregido> ObtenerProcesos()
        {
            return new List<ProcesoCorregido> {
                new ProcesoCorregido { Id = 1, Proceso = "Proceso1" },
                new ProcesoCorregido { Id = 2, Proceso = "Proceso2" },
                new ProcesoCorregido { Id = 3, Proceso = "Proceso3" }
            };
        }
    }
}
